using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Linq;

namespace SimpleFTP
{
    class Server
    {
        private TcpListener listener;
        private CancellationTokenSource stopToken = new CancellationTokenSource();

        public Server(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }
        
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

                    switch(command)
                    {
                        case "1":
                            await writer.WriteLineAsync(List(path));
                            break;

                        case "2":
                            var contentStream = File.OpenRead(path);
                            await writer.WriteLineAsync(contentStream.Length.ToString());
                            contentStream.CopyTo(writer.BaseStream);
                            contentStream.Close();
                            break;

                        default:
                            await writer.WriteLineAsync("Command is not found.");
                            break;
                    }

                }
            } 
        }

        private (string, string) ParseMessage(string str)
        {
            var tempStr = str.Split();
            return (tempStr[0],tempStr[1]);
        }

        private string List(string path)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(path);
                var filesName = directoryInfo.GetFiles();
                var directorysName = directoryInfo.GetDirectories();

                return filesName.Length + directorysName.Length + " "
                    + string.Join("", filesName.Select(name => $"{name.Name} false "))
                    + string.Join("", directorysName.Select(name => $"{name.Name} True "));
            }
            catch
            {
                return "-1";
            }
        }
    }
}
