using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyLazy;
using System.Collections.Generic;
using System.Threading;

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
            for (var i = 0; i < 5; i++)
            {
                Thread devThread = new Thread(() => lazy.Get());
                threadArr.Add(devThread);
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

            Assert.IsTrue(oldTime < 0.01);
        } 
        
        [TestMethod]
        public void GetReturnSameObject()
        {
            var lazyObject = LazyFactory<object>.CreateMymultithreadedLazy(() => new object());

            var threadMass = new List<Thread>();

            var results = new object[5];

            for (var i = 0; i < 5; ++i)
            {
                var local = i;
                Thread devThread = new Thread(() => results[local] = lazyObject.Get());
                threadMass.Add(devThread);
                threadMass[i].Start();
            }

            foreach (var thread in threadMass)
            {
                thread.Join();
            }

            for (var i = 0; i < results.Length - 1; ++i)
            {
                Assert.IsTrue(results[i] == results[i + 1]);
            }

        }
    }
}
