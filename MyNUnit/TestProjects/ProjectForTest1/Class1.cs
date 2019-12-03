using MyNUnit.Attributes;

namespace ProjectForTest1
{
    public class Class1
    {
        public static int count;

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
