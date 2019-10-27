using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleFTP
{
    /// <summary>
    /// Class for generating requests to the server
    /// </summary>
    public class Client
    {
        private TcpClient client;
        private StreamWriter writer;
        private StreamReader reader;

        public Client(string host, int port)
        {
            client = new TcpClient(host, port);
            var stream = client.GetStream();

            writer = new StreamWriter(stream) { AutoFlush = true };

            reader = new StreamReader(stream);
        }

        /// <summary>
        /// Closed client connection
        /// </summary>
        public void Close()
        {
            client.Close();
        }

        /// <summary>
        /// List command, returns server response
        /// </summary>
        public async Task<string> ListCommand(string path)
        {
            await writer.WriteLineAsync("1 " + path);
            return await reader.ReadLineAsync();
        }

        /// <summary>
        /// Get command, server response as file size and byte array itself
        /// </summary>
        public async Task<(long, byte[])> GetCommand(string path)
        {
            await writer.WriteLineAsync("2 " + path);

            var size = long.Parse(await reader.ReadLineAsync());

            if (size == -1)
            {
                return (-1, null);
            }
  
            var content = new byte[size];
            await reader.BaseStream.ReadAsync(content);

            return (size, content);
        }
    }
}
