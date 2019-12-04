using System;
using System.IO;

namespace MyNUnit
{
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
                MyNUnitRunner.Run(path);
                MyNUnitRunner.PrintResultTesting();
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
            catch (AggregateException exception)
            {
                var invalidOperationExcaption = exception.InnerException?.InnerException;
                if (invalidOperationExcaption.GetType() == typeof(InvalidOperationException))
                {
                    Console.WriteLine($"Error occured: {invalidOperationExcaption.Message}");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
