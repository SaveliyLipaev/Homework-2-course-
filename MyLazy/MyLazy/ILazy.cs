using System;
using System.Collections.Generic;
using System.Text;

namespace MyLazy
{
    /// <summary>
    /// Интерфейс, представляющий ленивое вычисление
    /// </summary>
    public interface ILazy<T>
    {
        T Get();
    }
}
