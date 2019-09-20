using System;
using System.Collections.Generic;
using System.Text;

namespace MyLazy
{
    public interface ILazy<T>
    {
        T Get();
    }
}
