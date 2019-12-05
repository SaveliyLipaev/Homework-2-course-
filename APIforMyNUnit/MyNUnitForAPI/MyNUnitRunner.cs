using MyNUnit.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        public static AsyncObservableCollection<TestInformation> TestsInformation { get; set; }

        private static ConcurrentBag<TestInformation> _succeeded;
        private static ConcurrentBag<TestInformation> _failed;
        private static ConcurrentBag<TestInformation> _ignored;

        public static IReadOnlyCollection<TestInformation> Succeeded => _succeeded;
        public static IReadOnlyCollection<TestInformation> Failed => _failed;
        public static IReadOnlyCollection<TestInformation> Ignored => _ignored;

        public static void Run<T>(List<Assembly> assemblies, Func<T> func) 
        {
            var types = assemblies.ToHashSet().SelectMany(a => a.ExportedTypes);
            TestsInformation = new AsyncObservableCollection<TestInformation>();
            TestsInformation.CollectionChanged += (obj, args) => func();
            _succeeded = new ConcurrentBag<TestInformation>();
            _failed = new ConcurrentBag<TestInformation>();
            _ignored = new ConcurrentBag<TestInformation>();
            Parallel.ForEach(types, TryExecuteAllTestMethods);
        }

        /// <summary>
        /// Running methods with beforeclass, test, afterclass attributes
        /// </summary>
        private static void TryExecuteAllTestMethods(Type type)
        {
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
                TestsInformation.Add(new TestInformation(methodInfo.Name, methodInfo.DeclaringType.FullName,
                    0, false, ignore: attributes.Ignore));
                _ignored.Add(new TestInformation(methodInfo.Name, methodInfo.DeclaringType.FullName,
                    0, false, ignore: attributes.Ignore));
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
                if (isCrashed)
                {
                    _failed.Add(new TestInformation(methodInfo.Name, methodInfo.DeclaringType.FullName,
                    watch.ElapsedMilliseconds, !isCrashed, attributes.Expected, attributes.Ignore));
                }
                else
                {
                    _succeeded.Add(new TestInformation(methodInfo.Name, methodInfo.DeclaringType.FullName,
                    watch.ElapsedMilliseconds, !isCrashed, attributes.Expected, attributes.Ignore));
                }
                TestsInformation.Add(new TestInformation(methodInfo.Name, methodInfo.DeclaringType.FullName,
                    watch.ElapsedMilliseconds, !isCrashed, attributes.Expected, attributes.Ignore));
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
    }
}
