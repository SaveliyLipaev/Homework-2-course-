using System;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// A method marked with this annotation is considered a test
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAttribute : Attribute
    {
        public Type Expected { get; set; }
        public string Ignore { get; set; }
    }
}