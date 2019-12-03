using MyNUnit.Attributes;
using System;

namespace ProjectForTest7
{
    public class Class7
    {
        [Test(Expected = typeof(NullReferenceException))]
        public void ExceptionTest()
        {
            throw new NullReferenceException();
        }
    }
}
