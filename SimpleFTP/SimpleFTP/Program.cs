using System;
using System.Threading;
using System.Collections.Generic;

namespace SimpleFTP
{
    class Program
    {
        static async System.Threading.Tasks.Task Main()
        {
            var server = new Server(1333);
            await server.Start();
        }
    }
}