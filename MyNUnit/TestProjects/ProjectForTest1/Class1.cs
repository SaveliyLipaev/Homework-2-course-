using MyNUnit.Attributes;
using System;

namespace ProjectForTest1
{
    public class Class1
    {
        public int count;

        [Test]
        public void Test1()
        {
            count++;
        }

        [Test]
        public void Test2()
        {
            count++;
        }
    }
}
