using System;
using System.Collections.Concurrent;
using System.Threading;


namespace MyThreadPool
{
    /// <summary>
    /// Сlass implementing a simple thread pool.
    /// </summary>
    public class MyThreadPool
    {
        private CancellationTokenSource stopToken = new CancellationTokenSource();
        private BlockingCollection<Action> queueTask = new BlockingCollection<Action>();

        public int NumberOfThreads { get; }

        public MyThreadPool(int numberOfThreads)
        {
            NumberOfThreads = numberOfThreads;
            CreateThreads(numberOfThreads);
        }

        /// <summary>
        /// Adding a new task to the thread pool
        /// </summary>
        public IMyTask<TResult> AddTask<TResult>(Func<TResult> func)
        {
            if (stopToken.Token.IsCancellationRequested)
            {
                throw new InvalidOperationException("Thread pool has been shutted down");
            }

            var task = new MyTask<TResult>(func, this);

            try
            {
                queueTask.Add(task.Calculate, stopToken.Token);
            }
            catch
            {
                throw new Exception("Thread pool has been shutted down");
            }

            return task;
        }

        /// <summary>
        /// Prohibition to add new tasks.
        /// </summary>
        public void Shutdown()
        {
            stopToken.Cancel();
            queueTask?.CompleteAdding();
            queueTask = null;
        }

        /// <summary>
        /// Thread creation.
        /// </summary>
        private void CreateThreads(int numberOfThreads)
        {   
            for (var i = 0; i < numberOfThreads; ++i)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        if (stopToken.IsCancellationRequested)
                        {
                            break;
                        }

                        queueTask?.Take().Invoke();
                    }
                }).Start();
            }
        }
    }
}
