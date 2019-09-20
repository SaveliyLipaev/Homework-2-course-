using System;
using System.Collections.Generic;
using System.Text;

namespace MyLazy
{
    public class MySimpleLazy<T> : ILazy<T>
    {
        private T result;
        private Func<T> supplier;
        private bool counted = false; 
     
        public MySimpleLazy(Func<T> supplier)
        {
            this.supplier = supplier;
        }

        public T Get()
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