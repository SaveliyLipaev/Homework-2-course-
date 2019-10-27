using System;
using System.IO;
using System.Net.Sockets;

namespace SimpleFTP
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var server = new Server(1333);
            server.Start();

            var client = new Client("localhost", 1333);


            var (size, content) = await client.GetCommand("../../../Program.cs");
            Console.WriteLine(size);
            foreach (var i in content)
            {
                Console.Write(i);
            }

        }
    }
}
