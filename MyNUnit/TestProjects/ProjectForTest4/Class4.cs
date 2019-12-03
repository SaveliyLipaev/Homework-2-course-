using MyNUnit.Attributes;

namespace ProjectForTest4
{
    public class Class4
    {
        public static bool ignored;

        [Test(Ignore = "just ignore")]
        public void IgnoredTest()
        {
            ignored = false;
        }
    }
}
