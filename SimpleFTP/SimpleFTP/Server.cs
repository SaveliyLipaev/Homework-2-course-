using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleFTP
{
    public class Server
    {
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
                while (!stopToken.IsCancellationRequested)
                {
                    var client = await listener.AcceptTcpClientAsync();

                    await Task.Run(() => ServiceClient(client));
                }
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
                    var message = await reader.ReadLineAsync();

                    var (command, path) = ParseMessage(message);

                    if (command == null || path == null)
                    {
                        await writer.WriteLineAsync("Error command.");
                        continue;
                    }

                    switch (command)
                    {
                        case "1":
                            await writer.WriteLineAsync(List(path));
                            break;

                        case "2":
                            try
                            {
                                var contentStream = File.OpenRead(path);
                                await writer.WriteLineAsync(contentStream.Length.ToString());
                                contentStream.CopyTo(writer.BaseStream);
                                contentStream.Close();
                            }
                            catch (Exception ex) when (ex is ArgumentException || ex is PathTooLongException || 
                                ex is DirectoryNotFoundException || ex is FileNotFoundException)
                            {
                                await writer.WriteLineAsync("-1");
                            }
                            catch
                            {
                                await writer.WriteLineAsync("Error command.");
                            }
                            break;

                        default:
                            await writer.WriteLineAsync("Command is not found.");
                            break;
                    }

                }
            }
        }

        /// <summary>
        /// Parsing client messages
        /// </summary>
        private (string, string) ParseMessage(string str)
        {
            var tempStr = str.Split();

            if (tempStr.Length != 2)
            {
                return (null, null);
            }

            return (tempStr[0], tempStr[1]);
        }

        /// <summary>
        /// Forming a response to the list request
        /// </summary>
        private string List(string path)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(path);
                var filesName = directoryInfo.GetFiles();
                var directorysName = directoryInfo.GetDirectories();

                return filesName.Length + directorysName.Length + " "
                    + string.Join("", filesName.Select(name => $"{name.Name} False "))
                    + string.Join("", directorysName.Select(name => $"{name.Name} True "));
            }
            catch
            {
                return "-1";
            }
        }
    }
}
