using System;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// The method marked with this annotation will work after each test
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterAttribute : Attribute
    {
    }
}