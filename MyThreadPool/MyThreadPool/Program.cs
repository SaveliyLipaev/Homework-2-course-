using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            var threadPool = new MyThreadPool(5);

            var task = threadPool.AddTask(() => true);
            Thread.Sleep(100);
            var flag = false;
            task.ContinueWith((x) =>
            {
                flag = x;
                return x;
            });

            Console.WriteLine(flag);
            Console.Read();

        }
    }
}
