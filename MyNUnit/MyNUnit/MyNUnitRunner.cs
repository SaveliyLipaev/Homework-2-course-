using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MyNUnit.Attributes;

namespace MyNUnit
{
    public static class MyNUnitRunner
    {
        public static BlockingCollection<string> Logger { get; private set; }

        public static void Run(string path)
        {
            var types = Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories).Select(Assembly.LoadFrom).ToHashSet().SelectMany(a => a.ExportedTypes);
            Logger = new BlockingCollection<string>();
            Parallel.ForEach(types, TryExecuteAllTestMethods);
        }

        private static void TryExecuteAllTestMethods(Type type)
        {
            ExecuteAllMethodWithAttribute<BeforeClassAttribute>(type);
            ExecuteAllMethodWithAttribute<TestAttribute>(type);
            ExecuteAllMethodWithAttribute<AfterClassAttribute>(type);
        }

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

        private static void ExecuteTestMethod(MethodInfo methodInfo)
        {
            if (!CheckMethod(methodInfo)) return;

            var attributes = Attribute.GetCustomAttribute(methodInfo, typeof(TestAttribute)) as TestAttribute;

            if (attributes.Ignore != null)
            {
                Logger.Add($"Test method {methodInfo.Name} ignored because {attributes.Ignore}.");
                return;
            }

            var constructor = methodInfo.DeclaringType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                Logger.Add($"Test class {methodInfo.DeclaringType.Name} should have parameterless constructor");
                return;
            }

            var instance = constructor.Invoke(null);

            ExecuteAllMethodWithAttribute<BeforeAttribute>(methodInfo.DeclaringType, instance);

            var watch = Stopwatch.StartNew();
            bool isCrashed = true;
            try
            {
                methodInfo.Invoke(instance, null);
                isCrashed = false;
            }
            catch (Exception ex)
            {
                if (attributes.Expected != null && attributes.Expected == ex.InnerException.GetType())
                {
                    isCrashed = false;
                }
            }
            finally
            {
                watch.Stop();
                Logger.Add($"Test method {methodInfo.DeclaringType.FullName}.{methodInfo.Name} {(isCrashed ? "failed" : "succeeded")}. Time: {watch.ElapsedMilliseconds} ms");
            }

            ExecuteAllMethodWithAttribute<AfterAttribute>(methodInfo.DeclaringType, instance);
        }

        private static void ExecuteOtherMethod(MethodInfo methodInfo, object instance, Type attribute)
        {
            if (!CheckMethod(methodInfo)) return;

            if ((attribute == typeof(BeforeClassAttribute) || attribute == typeof(AfterClassAttribute)) && !methodInfo.IsStatic)
            {
                Logger.Add($"Error:Methods marked with the BeforeClass or AfterClass attribute can only be static. {methodInfo.Name} not static.");
            }

            methodInfo.Invoke(instance, null);
        }

        private static bool CheckMethod(MethodInfo methodInfo)
        {
            if (methodInfo.GetParameters().Length > 0)
            {
                Logger.Add($"Method {methodInfo.Name} cannot receive parameters.");
                return false;
            }
            else if (methodInfo.ReturnType != typeof(void))
            {
                Logger.Add($"Menthod {methodInfo.Name} cannot return value.");
                return false;
            }
            return true;
        }
    }
}
