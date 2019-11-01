using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleFTP
{
    /// <summary>
    /// Class implementing simple ftp
    /// </summary>
    public class Server
    {
        private StreamWriter writer;
        private StreamReader reader;
        private TcpListener listener;
        private CancellationTokenSource stopToken = new CancellationTokenSource();

        public Server(int port)
        {
            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
            {
                throw new ArgumentOutOfRangeException();
            }
            listener = new TcpListener(IPAddress.Any, port);
        }

        /// <summary>
        /// Stop server
        /// </summary>
        public void Stop()
        {
            stopToken.Cancel();
        }

        /// <summary>
        /// Server start
        /// </summary>
        public async Task Start()
        {
            listener.Start();

            try
            {
                var client = await listener.AcceptTcpClientAsync();

                await Task.Run(() => ServiceClient(client));
            }
            finally
            {
                listener.Stop();
            }
        }

        /// <summary>
        /// Сustomer request service
        /// </summary>
        private async Task ServiceClient(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                var writer = new StreamWriter(stream) { AutoFlush = true };
                var reader = new StreamReader(stream);

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
    }
}
