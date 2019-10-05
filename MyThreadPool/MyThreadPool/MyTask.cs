using System;
using System.Threading;

namespace MyThreadPool
{
    public class MyTask<TResult> : IMyTask<TResult>
    {
        private Func<TResult> function;
        private AutoResetEvent waitHandler = new AutoResetEvent(false);
        private AggregateException exception;
        public bool IsComleted { get; private set; } = false;

        public TResult Result 
        { 
            get
            {
                waitHandler.WaitOne();
                if (exception != null)
                {
                    return Result;
                }

                throw exception;
            }
            private set => Result = value; 
        }


        public MyTask(Func<TResult> task)
        {
            function = task;
        }

        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
        {
            var newTask = new MyTask<TNewResult>(() => func(Result));
            new Thread(newTask.Calculate);
            return newTask;
        }

        public void Calculate()
        {
            try
            {
                Result = function();
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
