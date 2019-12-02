using MyNUnit.Attributes;
using System;

namespace ProjectForTest2
{
    public class Class2
    {
        public int count = 0;
        [Before]
        public void Before1()
        {
            count++;
        }

        [Before]
        public void Before2()
        {
            count++;
        }

        [Test]
        public void Test1()
        {
        }

        [Test]
        public void Test2()
        {
        }

        [After]
        public void After()
        {
            count++;
        }
    }
}
