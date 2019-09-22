using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MyLazy;
using System.Threading;
using System.Collections.Generic;

namespace MyLazyTests
{
    [TestClass]
    public class MyMultithreadedLazyTests
    {
        private MyMultithreadedLazy<int> lazy;
        private List<Thread> threadArr;
        private int result = 0;

        [TestInitialize]
        public void Initialize()
        {
            lazy = LazyFactory<int>.CreateMymultithreadedLazy(() =>
            {
                for (var i = 0; i < 10; i++)
                {
                    result += i;
                }
                return result;
            });

            threadArr = new List<Thread>();
            for (int i = 0; i < 5; i++)
            {
                Thread devthread = new Thread(() => lazy.Get());
                threadArr.Add(devthread);
            }
        }

        [TestMethod]
        public void GetTests()
        {

            foreach (var thr in threadArr)
            {
                thr.Start();
            }
            foreach (var thr in threadArr)
            {
                thr.Join();
            }
            Assert.AreEqual(45, result);
        }

        [TestMethod]
        public void GetTimeSecondsCallTests()
        {
            var myStopwatchSecondGet = new System.Diagnostics.Stopwatch();
            threadArr[0].Start();

            myStopwatchSecondGet.Start();
            for (var i = 1; i < 5; i++)
            {
                threadArr[i].Start();
            }
            foreach (var thr in threadArr)
            {
                thr.Join();
            }
            myStopwatchSecondGet.Stop();

            double oldTime = myStopwatchSecondGet.Elapsed.Seconds + (double)myStopwatchSecondGet.Elapsed.Milliseconds / 60;

            Assert.IsTrue(oldTime < 0.00001);
        }
    }
}
