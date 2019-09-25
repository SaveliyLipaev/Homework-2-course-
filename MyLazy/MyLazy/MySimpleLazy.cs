using System;

namespace MyLazy
{
    /// <summary>
    /// Простая версия Lazy с гарантией корректной работы в однопоточном режиме
    /// </summary>
    public class MySimpleLazy<T> : ILazy<T>
    {
        /// <summary>
        /// Переменная, хранящая результат вычисления
        /// </summary>
        private T result;

        /// <summary>
        /// Объект, предоставляющий вычисление
        /// </summary>
        private Func<T> supplier;

        /// <summary>
        /// Флаг, если вычисление уже произошло, то true
        /// </summary>
        private bool counted = false;

        public MySimpleLazy(Func<T> supplier)
        {
            this.supplier = supplier;
        }

        /// <summary>
        /// Возвращает вычисленное значение
        /// </summary>
        public T Get()
        {
            if (counted)
            {
                return result;
            }
            counted = true;
            return result = supplier();
        }
    }
}