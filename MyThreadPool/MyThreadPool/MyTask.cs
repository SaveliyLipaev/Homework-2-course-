using System;
using System.Threading;

namespace MyThreadPool
{
    /// <summary>
    /// Class that implements the task.
    /// </summary>
    public class MyTask<TResult> : IMyTask<TResult>
    {
        private MyThreadPool threadPool;
        private Func<TResult> function;
        private AutoResetEvent waitHandler = new AutoResetEvent(false);
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
            catch(Exception ex) 
            {
                exception = new AggregateException(ex);
            }

            IsComleted = true;
            function = null;
            waitHandler.Set();
        }
    }
}
