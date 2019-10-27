using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFTP
{
    class Client
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

        public async Task<string> ListCommand(string path)
        {
            await writer.WriteLineAsync("1 " + path);
            return await reader.ReadLineAsync();
        }

        public async Task<(long, byte[])> GetCommand(string path)
        {
            await writer.WriteLineAsync("2 " + path);
            var size = long.Parse(await reader.ReadLineAsync());

            var content = new byte[size];
            await reader.BaseStream.ReadAsync(content);

            return (size, content);
        }
    }
}
