using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MyLazy;

namespace MyLazyTests
{
    [TestClass]
    public class MyMultithreadedLazyTests
    {
        private MySimpleLazy<int> simpleLazy;

        [TestInitialize]
        public void Initialize()
        {
            simpleLazy = LazyFactory<int>.CreateSimpleLazy(() =>
            {
                int result = 0;
                for (var i = 0; i < 100; i++)
                {
                    result += i;
                }
                return result;
            });
        }

        [TestMethod]
        public void GetTests()
        {
            Assert.AreEqual(4950, simpleLazy.Get());
        }
    }
}
