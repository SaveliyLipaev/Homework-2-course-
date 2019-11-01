using SimpleFTP;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Chat
{
    class OnlineChat
    {
        private Client client;
        private Server server;
        private bool IsServer;

        public bool ChatIsLife { get; private set; } = false;

        public OnlineChat(string[] args)
        {
            if (args.Length == 2)
            {
                client = new Client(args[0], Convert.ToInt32(args[1]));
                client.Connect();
                IsServer = false;
            }
            else if (args.Length == 1)
            {
                server = new Server(Convert.ToInt32(args[0]));
                server.Start();
                IsServer = true;
            }
            else
            {
                throw new InvalidOperationException();
            }

            ChatIsLife = true;
        }

        /// <summary>
        /// Sending message
        /// </summary>
        public async Task Write(string message)
        {
            if (IsServer)
            {
                try
                {
                    await server.WriteMessage(message);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (message == "exit")
                {
                    server.Stop();
                }
            }
            else if (!IsServer)
            {
                try
                {
                    await client.WriteMessage(message);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (message == "exit")
                {
                    client.Close();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
