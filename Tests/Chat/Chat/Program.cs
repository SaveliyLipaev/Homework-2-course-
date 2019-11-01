using System;
using System.Threading.Tasks;

namespace Chat
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var chat = new OnlineChat(args);

            var client = new Client("localhost", 4000);

            client.Connect();

            while (chat.ChatIsLife)
            {
                var message = Console.ReadLine();
                await chat.Write(message);
            }
        }
    }
}
