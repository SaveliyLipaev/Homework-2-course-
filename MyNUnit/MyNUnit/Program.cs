using System;

namespace MyNUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            MyNUnitRunner.Run(args[0]);
            foreach (var str in MyNUnitRunner.Logger)
            {
                Console.WriteLine(str);
            }
            var f = MyNUnitRunner.Logger;
        }
    }
}
