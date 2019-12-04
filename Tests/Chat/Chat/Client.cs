using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Chat
{
    /// <summary>
    /// Class implementing client
    /// </summary>
    public class Client
    {
        private TcpClient client;
        private StreamWriter writer;
        private StreamReader reader;
        private CancellationTokenSource stopToken = new CancellationTokenSource();
        private string host;
        private int port;

        public bool IsConnected { get; private set; } = false;

        public Client(string host, int port)
        {
            client = new TcpClient();
            this.host = host;
            this.port = port;
        }

        /// <summary>
        /// Client connection to server
        /// </summary>
        public async Task<bool> Connect()
        {
            var delay = TimeSpan.FromSeconds(1);
            int counter = 0;
            for (var i = 0; i < 3; ++i)
            {
                try
                {
                    client.Connect(host, port);
                    break;
                }
                catch
                {
                    ++counter;
                }
                await Task.Delay(delay);
            }

            if (counter != 3)
            {
                var stream = client.GetStream();
                writer = new StreamWriter(stream) { AutoFlush = true };
                reader = new StreamReader(stream);
                Task.Run(async () => await ServerListener());
                return IsConnected = true;
            }

            return false;
        }

        public async Task ServerListener()
        {
            while (!stopToken.IsCancellationRequested)
            {
                var message = await ReadMessage();
                if (message == "exit")
                {
                    stopToken.Cancel();
                    break;
                }
                Console.WriteLine(message);
            }
        }

        /// <summary>
        /// Method for sending a message to the server, with repeated requests 
        /// if a network error occurs
        /// </summary>
        public async Task WriteMessage(string message)
        {
            var delay = TimeSpan.FromSeconds(1);
            for (var i = 0; i < 3; ++i)
            {
                try
                {
                    await writer.WriteLineAsync(message);
                    return;
                }
                catch { }
                await Task.Delay(delay);
            }

            await writer.WriteLineAsync(message);
        }

        /// <summary>
        /// Method for receiving messages from the server, 
        /// with repeated requests if a network error occurs
        /// </summary>
        private async Task<string> ReadMessage()
        {
            var delay = TimeSpan.FromSeconds(1);
            for (var i = 0; i < 3; ++i)
            {
                try
                {
                    return await reader.ReadLineAsync();
                }
                catch { }
                await Task.Delay(delay);
            }

            return await reader.ReadLineAsync();
        }

        /// <summary>
        /// Closed client connection
        /// </summary>
        public void Close()
        {
            client.Close();
        }

    }
}
