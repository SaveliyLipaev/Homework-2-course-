using ProjectForTest1;
using ProjectForTest2;
using ProjectForTest4;
using System;
using Xunit;

namespace MyNUnit.Test
{
    public class MyNUnitTests
    {
        [Fact]
        public void WithoutOtherAttributesSimpleTest()
        {
            Class1.count = 8;
            MyNUnitRunner.Run("..\\..\\..\\..\\TestProjects\\ProjectForTest1");
            Assert.Equal(10, Class1.count);
        }

        [Fact]
        public void SimpleWithAttributesBeforeAndAfterTest()
        {
            Class2.array = new bool[] { false, false, false, false };
            MyNUnitRunner.Run("..\\..\\..\\..\\TestProjects\\ProjectForTest2");
            Assert.True(Class2.array[0]);
            Assert.True(Class2.array[1]);
            Assert.False(Class2.array[2]);
            Assert.True(Class2.array[3]);
        }

        [Fact]
        public void FailedDueToExceptionTest()
        {
            MyNUnitRunner.Run("..\\..\\..\\..\\TestProjects\\ProjectForTest3");
            Assert.False(MyNUnitRunner.TestInformation.Take().IsPassed);
        }

        [Fact]
        public void IgnoreAnnotationsTest()
        {
            Class4.ignored = true;
            MyNUnitRunner.Run("..\\..\\..\\..\\TestProjects\\ProjectForTest4");
            Assert.True(Class4.ignored);
        }

        [Fact]
        public void IgnoreAnnotationsMessageTest()
        {
            MyNUnitRunner.Run("..\\..\\..\\..\\TestProjects\\ProjectForTest4");
            Assert.Equal("just ignore", MyNUnitRunner.TestInformation.Take().Ignore);
        }

        [Fact]
        public void ConstructorWithParametersTest()
        {
            Assert.Throws<AggregateException>(() => MyNUnitRunner.Run("..\\..\\..\\..\\TestProjects\\ProjectForTest5"));
        }

        [Fact]
        public void MethodWithParametersTest()
        {
            Assert.Throws<AggregateException>(() => MyNUnitRunner.Run("..\\..\\..\\..\\TestProjects\\ProjectForTest6"));
        }

        [Fact]
        public void MethodWithExpectedAnnotationsTest()
        {
            MyNUnitRunner.Run("..\\..\\..\\..\\TestProjects\\ProjectForTest7");
            Assert.True(typeof(NullReferenceException) == MyNUnitRunner.TestInformation.Take().Expected);
        }
    }
}
