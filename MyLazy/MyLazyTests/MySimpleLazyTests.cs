using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MyLazy;
using System.Threading;

namespace MyLazyTests
{
    [TestClass]
    public class MySimpleLazyTests
    {
        private MySimpleLazy<int> simpleLazy; 

        [TestInitialize]
        public void Initialize()
        {
            simpleLazy = LazyFactory<int>.CreateSimpleLazy(() =>
            {
                int result = 0;
                for (var i = 0; i < 10; i++)
                {
                    result += i;
                    Thread.Sleep(10);
                }
                return result;
            });
        }

        [TestMethod]
        public void GetTests()
        {
            Assert.AreEqual(45, simpleLazy.Get());
        }

        [TestMethod]
        public void GetTimeSecondCallTests()
        {
            var myStopwatchSecondGet = new System.Diagnostics.Stopwatch();
            simpleLazy.Get();

            myStopwatchSecondGet.Start();
            simpleLazy.Get();
            myStopwatchSecondGet.Stop();

            double oldTime = myStopwatchSecondGet.Elapsed.Seconds + (double)myStopwatchSecondGet.Elapsed.Milliseconds / 60;

            Assert.IsTrue(oldTime < 0.00001);
        }

        [TestMethod]
        public void GetReturnNull()
        {
            var lazy = LazyFactory<object>.CreateSimpleLazy(() => null);
            Assert.IsNull(lazy.Get());
        }


    }
}
