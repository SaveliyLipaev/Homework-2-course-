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

            int GetInt() => 57;
            var Task = threadPool.AddTask(GetInt);
            Console.WriteLine(Task.Result);

            Console.Read();

        }
    }
}
