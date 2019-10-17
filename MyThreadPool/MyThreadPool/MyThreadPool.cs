using System;
using System.Collections.Concurrent;
using System.Threading;


namespace MyThreadPool
{
    /// <summary>
    /// Сlass implementing a simple thread pool.
    /// </summary>
    public partial class MyThreadPool
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
                throw new InvalidOperationException("Thread pool has been shutted down");
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

        private class MyTask<TResult> : IMyTask<TResult>
        {
            private MyThreadPool threadPool;
            private Func<TResult> function;
            private ManualResetEvent waitHandler = new ManualResetEvent(false);
            private AggregateException exception;
            private TResult result;

            public bool IsComleted { get; private set; } = false;

            public TResult Result
            {
                get
                {
                    waitHandler.WaitOne();
                    if (exception == null)
                    {
                        return result;
                    }

                    throw exception;
                }
            }

            public MyTask(Func<TResult> task, MyThreadPool threadPool)
            {
                function = task;
                this.threadPool = threadPool;
            }

            /// <summary>
            /// Add new task based on result of this task.
            /// </summary>
            public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func) => threadPool.AddTask(() => func(Result));

            /// <summary>
            /// Calculate task and assings Result and IsCompleted properties.
            /// </summary>
            public void Calculate()
            {
                try
                {
                    result = function();    
                }
                catch (Exception ex)
                {
                    exception = new AggregateException(ex);
                }

                IsComleted = true;
                function = null;
                waitHandler.Set();
            }
        }
    }
}
