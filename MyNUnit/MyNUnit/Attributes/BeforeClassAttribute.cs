using System;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// The method marked with this annotation will work before testing
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeClassAttribute : Attribute
    {
    }
}