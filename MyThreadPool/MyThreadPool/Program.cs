using System;
using System.Threading;

namespace MyThreadPool
{       
    class Program
    {
        static void Main(string[] args)
        {
            var thrpool = new MyThreadPool(5);
            for (var i = 0; i < 10; ++i)
            {
                thrpool.AddTask(() => { 
                    Thread.Sleep(1000);
                    Console.WriteLine(i * 10);
                    return i;
                });
            }
            Thread.Sleep(10000);
            thrpool.Shutdown();
        }
    }
}
