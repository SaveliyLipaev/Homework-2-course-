using System;
using System.IO;

namespace MD5CheckSum
{
    /// <summary>
    /// Консольное приложение, вычисляющее check-сумму директории файловой системы
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Nothing is entered into the application");
                return;
            }

            var path = args[0];

            try
            {
                Console.WriteLine("Test execution has begun");
                CheckSumSimple.Run(path);
                CheckSumAsync.Run(path);
                Console.WriteLine("Test execution is over");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Error: Directory not found");
            }
            catch (IOException)
            {
                Console.WriteLine("Error: The path is the file name or something else");
            }

            Console.WriteLine($"Run time asynchronously {CheckSumAsync.TimeWork} and Check sum = {CheckSumAsync.Sum}");
            Console.WriteLine($"Runtime is not asynchronous {CheckSumSimple.TimeWork} and Check sum = {CheckSumSimple.Sum}");
        }
    }
}
