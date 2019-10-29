using System;
using System.Collections.Generic;
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
        private string host;
        private int port;

        public bool IsConnect { get; private set; } = false;

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
                delay *= 2;
            }

            if (counter != 3)
            {
                var stream = client.GetStream();
                writer = new StreamWriter(stream) { AutoFlush = true };
                reader = new StreamReader(stream);
                return IsConnect = true;
            }

            return false;
        }

        /// <summary>
        /// Method for sending a message to the server, with repeated requests 
        /// if a network error occurs
        /// </summary>
        private async Task WriteMessage(string message)
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
                delay *= 2;
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
                delay *= 2;
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

        /// <summary>
        /// List command, returns server response
        /// </summary>
        public async Task<(string, List<(string, bool)>)> ListCommand(string path)
        {
            if (!IsConnect)
            {
                throw new InvalidOperationException();
            }

            try
            {
                await WriteMessage("1 " + path);

                var message = await ReadMessage();

                if (message == "-1")
                {
                    return ("-1", null);
                }

                var splitMessage = message.Split();

                var list = new List<(string, bool)>();

                for (var i = 1; i < splitMessage.Length - 1; i += 2)
                {
                    list.Add((splitMessage[i], Convert.ToBoolean(splitMessage[i + 1])));
                }

                return (message, list);
            }
            catch (Exception ex)
            {
                return (ex.Message, null);
            }
        }

        /// <summary>
        /// Get command, server response as file size and byte array itself
        /// </summary>
        public async Task<(string, byte[], string)> GetCommand(string path)
        {
            if (!IsConnect)
            {
                throw new InvalidOperationException();
            }

            try
            {
                await WriteMessage("2 " + path);

                var size = long.Parse(await ReadMessage());

                if (size == -1)
                {
                    return ("-1", null, null);
                }

                var content = new byte[size];
                await reader.BaseStream.ReadAsync(content);

                return (size.ToString(), content, null);
            }
            catch (Exception ex)
            {
                return ("-1", null, ex.Message);
            }
        }
    }
}
