using SimpleFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var server = new Server(4000);
            server.Start();

            var chat = new OnlineChat(args);

            while (chat.ChatIsLife)
            {
                var message = Console.ReadLine();
                await chat.Write(message);
            }
        }
    }
}
