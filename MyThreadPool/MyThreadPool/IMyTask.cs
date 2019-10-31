using System;
using System.Collections.Generic;
using System.Text;

namespace MyThreadPool
{
    /// <summary>
    /// MyTask Interface
    /// </summary>
    public interface IMyTask<TResult>
    {
        /// <summary>
        /// Gets true if task is finished
        /// </summary>
        bool IsComleted { get; }

        /// <summary>
        /// Gets task result
        /// </summary>
        TResult Result { get; }

        /// <summary>
        /// Add new task based on result of this task
        /// </summary>
        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);
    }
}
