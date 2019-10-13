using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MyThreadPoolTests
{
    [TestClass]
    public class MyThreadPoolTests
    {
        private MyThreadPool.MyThreadPool threadPool;

        [TestInitialize]
        public void Initialize()
        {
            threadPool = new MyThreadPool.MyThreadPool(5);
        }

        [TestMethod]
        public void NumberOfThreads()
        {
            int numberOfEvaluatedTasks = 0;
            for (var i = 0; i < 10; ++i)
            {   
                threadPool.AddTask(() =>
                {
                    Interlocked.Increment(ref numberOfEvaluatedTasks);
                    Thread.Sleep(1000);
                    return 5;
                });
            }

            Thread.Sleep(500);
            Assert.IsTrue(numberOfEvaluatedTasks == threadPool.NumberOfThreads);
            threadPool.Shutdown();
        }

        [TestMethod]
        public void TaskWorkRight()
        {
            int GetInt() => 57;
            var Task = threadPool.AddTask(GetInt);
            Assert.AreEqual(GetInt(), Task.Result);
            threadPool.Shutdown();
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void NotAddToThreadPoolkAfterShutdown()
        {
            threadPool.Shutdown();

            threadPool.AddTask(() => 1);
        }

        [TestMethod]
        public void ShutdownDoesNotStopEvaluatingTasks()
        {
            int numberOfEvaluatedTasks = 0;

            for (var i = 0; i < 5; ++i)
            {
                threadPool.AddTask(() =>
                {
                    Interlocked.Increment(ref numberOfEvaluatedTasks);
                    Thread.Sleep(500);
                    return 5;
                });
            }

            Thread.Sleep(100);
            threadPool.Shutdown();

            Thread.Sleep(400);
            Assert.AreEqual(threadPool.NumberOfThreads, numberOfEvaluatedTasks);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void ResultThrowRightExceptionOnError()
        {
            var result = threadPool.AddTask<object>(() => throw new NullReferenceException()).Result;
        }


        [TestMethod]
        public void ContinueWithWork()
        {
            var task = threadPool.AddTask(() => true);
            var flag = false;
            task.ContinueWith((x) =>
            {
                flag = x;
                return x;
            });

            Thread.Sleep(400);
            Assert.IsTrue(flag);
        }
    }
}