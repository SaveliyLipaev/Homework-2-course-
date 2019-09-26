using System;
using System.Collections.Generic;
using System.Text;

namespace MyLazy
{
    /// <summary>
    /// Класс, возвращающий две разные реализации ILazy<T>
    /// </summary>
    public static class LazyFactory<T>
    {
        /// <summary>
        /// Возвращает простоую реализацию Lazy
        /// </summary>
        public static MySimpleLazy<T> CreateSimpleLazy(Func<T> supplier) => new MySimpleLazy<T>(supplier);

        /// <summary>
        /// Возвращает многопоточную реализацию Lazy
        /// </summary>
        public static MyMultithreadedLazy<T> CreateMymultithreadedLazy(Func<T> supplier) => new MyMultithreadedLazy<T>(supplier);
    }
}
