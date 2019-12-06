using System;

namespace MyNUnit.Attributes
{
    /// <summary>
    /// The method marked with this annotation will be triggered before each test
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeAttribute : Attribute
    {
    }
}