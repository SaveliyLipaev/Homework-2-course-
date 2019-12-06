using System;

namespace APIforMyNUnit.Models
{
    /// <summary>
    /// Class for storing the properties of the method marked by antotation test
    /// </summary>
    public class TestFullInformationModel
    {
        /// <summary>
        /// Property with the name of the method labeled annotation test
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// Property with the full name of the assembly to which this method belongs
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// Property containing an annotation argument expected for exceptions
        /// </summary>
        public Type Expected { get; }

        /// <summary>
        /// A property containing an annotation argument ignored to comment on why the test is skipped
        /// </summary>
        public string Ignore { get; }

        /// <summary>
        /// Method time in milliseconds
        /// </summary>
        public long Time { get; }

        /// <summary>
        /// True if the test passes
        /// </summary>
        public bool IsPassed { get; }

        public TestFullInformationModel(string methodName, string assemblyName, long time,
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
