using System;
using System.Collections.Generic;
using System.Text;

namespace MyThreadPool
{
    public interface IMyTask<TResult>
    {
        bool IsComleted { get; }
        TResult Result { get; }
        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);

    }
}
