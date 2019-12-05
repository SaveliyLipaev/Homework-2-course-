using APIforMyNUnit.Models;
using MyNUnit.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyNUnit
{
    /// <summary>
    /// Class that implements testing methods marked with annotation test along the specified path
    /// </summary>
    public static class MyNUnitRunner
    {
        public static AsyncObservableCollection<TestFullInformationModel> TestsInformation { get; private set; }
        public static ConcurrentDictionary<string, TestedAssemblyModel> AssemblyInformation { get; private set; }

        private static object locker = new object();

        public static void Run<T>(List<Assembly> assemblies, Func<T> func)
        {
            var types = assemblies.ToHashSet().SelectMany(a => a.ExportedTypes);
            TestsInformation = new AsyncObservableCollection<TestFullInformationModel>();
            AssemblyInformation = new ConcurrentDictionary<string, TestedAssemblyModel>();
            TestsInformation.CollectionChanged += (obj, args) => func();
            Parallel.ForEach(types, TryExecuteAllTestMethods);
        }

        /// <summary>
        /// Running methods with beforeclass, test, afterclass attributes
        /// </summary>
        private static void TryExecuteAllTestMethods(Type type)
        {
            AssemblyInformation.TryAdd(type.Assembly.FullName,
                new TestedAssemblyModel()
                {
                    Name = type.Assembly.GetName().Name,
                    Succeeded = new List<TestInformationModel>(),
                    Failed = new List<TestInformationModel>(),
                    Ignored = new List<TestInformationModel>(),
                });

            ExecuteAllMethodWithAttribute<BeforeClassAttribute>(type);
            ExecuteAllMethodWithAttribute<TestAttribute>(type);
            ExecuteAllMethodWithAttribute<AfterClassAttribute>(type);
        }

        /// <summary>
        /// Running methods with a specific attribute
        /// </summary>
        private static void ExecuteAllMethodWithAttribute<AttributeType>(Type type, object instance = null) where AttributeType : Attribute
        {
            var methodsWithAttribute = type.GetTypeInfo().DeclaredMethods.Where(mi => Attribute.IsDefined(mi, typeof(AttributeType)));

            Action<MethodInfo> RunMethod;

            switch (typeof(AttributeType))
            {
                case Type attribute when attribute == typeof(TestAttribute):
                    RunMethod = ExecuteTestMethod;
                    break;

                case Type attribute when attribute == typeof(BeforeClassAttribute) || attribute == typeof(AfterClassAttribute)
                        || attribute == typeof(BeforeAttribute) || attribute == typeof(AfterAttribute):
                    RunMethod = mi => ExecuteOtherMethod(mi, instance, attribute);
                    break;

                default:
                    throw new InvalidProgramException("Unpredictable attribute");
            }

            Parallel.ForEach(methodsWithAttribute, RunMethod);
        }

        /// <summary>
        /// Execution of the test method, before that it starts all the methods
        /// with the annotation before and after the test, all the methods with the annotation after
        /// </summary>
        private static void ExecuteTestMethod(MethodInfo methodInfo)
        {
            CheckMethod(methodInfo);

            var attributes = Attribute.GetCustomAttribute(methodInfo, typeof(TestAttribute)) as TestAttribute;

            if (attributes.Ignore != null)
            {
                PerpetuateData(methodInfo.Name, methodInfo.DeclaringType.Assembly.FullName,
                    0, false, ignore: attributes.Ignore);
                return;
            }

            var constructor = methodInfo.DeclaringType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                throw new InvalidOperationException($"Test class {methodInfo.DeclaringType.Name} should have parameterless constructor");
            }

            var instance = constructor.Invoke(null);

            ExecuteAllMethodWithAttribute<BeforeAttribute>(methodInfo.DeclaringType, instance);

            var watch = Stopwatch.StartNew();
            bool isCrashed = true;
            try
            {
                methodInfo.Invoke(instance, null);
                if (attributes.Expected == null)
                {
                    isCrashed = false;
                }
            }
            catch (Exception ex)
            {
                if (attributes.Expected == ex.InnerException.GetType())
                {
                    isCrashed = false;
                }
            }
            finally
            {
                watch.Stop();

                PerpetuateData(methodInfo.Name, methodInfo.DeclaringType.Assembly.FullName,
                    watch.ElapsedMilliseconds, isCrashed, attributes.Expected, attributes.Ignore);
            }

            ExecuteAllMethodWithAttribute<AfterAttribute>(methodInfo.DeclaringType, instance);
        }

        /// <summary>
        /// Execution of the method marked by antotations beforeclass, afterclass, before, after
        /// </summary>
        private static void ExecuteOtherMethod(MethodInfo methodInfo, object instance, Type attribute)
        {
            CheckMethod(methodInfo);

            if ((attribute == typeof(BeforeClassAttribute) || attribute == typeof(AfterClassAttribute)) && !methodInfo.IsStatic)
            {
                throw new InvalidOperationException($"Error:Methods marked with the BeforeClass or AfterClass attribute can only be static. {methodInfo.Name} not static.");
            }

            methodInfo.Invoke(instance, null);
        }

        /// <summary>
        /// Checking the method so that it would be without input parameters and return nothing
        /// </summary>
        private static void CheckMethod(MethodInfo methodInfo)
        {
            if (methodInfo.GetParameters().Length > 0)
            {
                throw new InvalidOperationException($"Method {methodInfo.Name} cannot receive parameters.");
            }
            else if (methodInfo.ReturnType != typeof(void))
            {
                throw new InvalidOperationException($"Menthod {methodInfo.Name} cannot return value.");
            }
        }

        /// <summary>
        /// Method for adding test information to general information about testing and testing specific assemblies
        /// </summary>
        private static void PerpetuateData(string methodName, string assemblyName, long time, bool isCrashed, Type expected = null, string ignore = null)
        {
            lock (locker)
            {
                TestsInformation.Add(new TestFullInformationModel(methodName, assemblyName,
                    time, !isCrashed, expected, ignore));

                AssemblyInformation.TryRemove(assemblyName, out TestedAssemblyModel testedAssemblyModel);

                if (ignore != null)
                {
                    testedAssemblyModel.Ignored.Add(new TestInformationModel() { Name = methodName, Ignore = ignore, Time = 0 });
                }
                else if (isCrashed)
                {
                    testedAssemblyModel.Failed.Add(new TestInformationModel() { Name = methodName, Time = time });
                }
                else
                {
                    testedAssemblyModel.Succeeded.Add(new TestInformationModel() { Name = methodName, Time = time });
                }

                AssemblyInformation.TryAdd(assemblyName, testedAssemblyModel);
            }
        }
    }
}
