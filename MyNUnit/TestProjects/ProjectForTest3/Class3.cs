using MyNUnit.Attributes;
using System;

namespace ProjectForTest3
{
    public class Class3
    {
        [Test]
        public void FailedTest()
        {
            throw new Exception();
        }
    }
}
