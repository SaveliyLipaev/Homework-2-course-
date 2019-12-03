using System;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// The method marked with this annotation will work after testing
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterClassAttribute : Attribute
    {
    }
}