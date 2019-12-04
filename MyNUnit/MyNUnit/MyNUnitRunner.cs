using MyNUnit.Attributes;
using System;
using System.Collections.Concurrent;
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
        public static BlockingCollection<TestInformation> TestInformation { get; private set; }

        public static void Run(string path)
        {
            var types = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories).Select(Assembly.LoadFrom).ToHashSet().SelectMany(a => a.ExportedTypes);
            TestInformation = new BlockingCollection<TestInformation>();
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
                TestInformation.Add(new TestInformation(methodInfo.Name, methodInfo.DeclaringType.FullName,
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
                TestInformation.Add(new TestInformation(methodInfo.Name, methodInfo.DeclaringType.FullName,
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
