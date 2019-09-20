using System;
using System.Collections.Generic;
using System.Text;

namespace MyLazy
{
    public class MyMultithreadedLazy<T> : ILazy<T>
    {
        private T result;
        private Func<T> supplier;
        private bool counted = false;
        private object locker = new object();

        public MyMultithreadedLazy(Func<T> supplier)
        {
            this.supplier = supplier;
        }

        public T Get()  
        {
            lock (locker)
            {
                if (counted)
                {
                    return result;
                }
                else
                {
                    counted = true;
                    return result = supplier();
                }
            }
        }
    }
}
