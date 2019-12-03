using System;

namespace MyNUnit
{
    /// <summary>
    /// Class for storing the properties of the method marked by antotation test
    /// </summary>
    public class TestInformation
    {
        public string MethodName { get; }

        public string AssemblyName { get; }

        public Type Expected { get; }

        public string Ignore { get; }

        public long Time { get; }

        public bool IsPassed { get; }

        public TestInformation(string methodName, string assemblyName, long time,
            bool isPassed, Type expected = null, string ignore = null)
        {
            MethodName = methodName;
            AssemblyName = assemblyName;
            Expected = expected;
            Ignore = ignore;
            Time = time;
            IsPassed = isPassed;
        }
    }
}
