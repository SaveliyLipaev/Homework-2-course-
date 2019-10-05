using System;
using System.Collections.Concurrent;
using System.Threading;


namespace MyThreadPool
{
    public class MyThreadPool
    {
        private CancellationTokenSource stopToken = new CancellationTokenSource();
        private BlockingCollection<Action> queueTask = new BlockingCollection<Action>();

        public MyThreadPool(int numberOfThreads)
        {
            CreateThreads(numberOfThreads);
        }

        public void AddTask<TResult>(Func<TResult> task)
        {
            if (stopToken.Token.IsCancellationRequested)
            {
                return;
            }
            queueTask.Add(new MyTask<TResult>(task).Calculate);
        }

        public void Shutdown()
        {
            stopToken.Cancel();
            queueTask = null;
        }

        private void CreateThreads(int numberOfThreads)
        {   
            for (var i = 0; i < numberOfThreads; ++i)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        if (stopToken.Token.IsCancellationRequested)
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
