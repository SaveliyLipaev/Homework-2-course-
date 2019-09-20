using System;
using System.Collections.Generic;
using System.Text;

namespace MyLazy
{
    public static class LazyFactory<T>
    {
        public static MySimpleLazy<T> CreateSimpleLazy<T>(Func<T> supplier)
        {
            return new MySimpleLazy<T>(supplier);
        }

        public static MyMultithreadedLazy<T> CreateMymultithreadedLazy<T>(Func<T> supplier)
        {
            return new MyMultithreadedLazy<T>(supplier);
        }
    }
}
