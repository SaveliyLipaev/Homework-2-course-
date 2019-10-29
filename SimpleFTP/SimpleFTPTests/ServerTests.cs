using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SimpleFTP.Tests
{
    [TestClass()]
    public class ServerTests
    {
        private Server server;
        private Client client;
        private string path = "../../../../SimpleFTPTests/TestFiles";

        [TestInitialize]
        public void Initialize()
        {
            server = new Server(1333);
            server.Start();
            client = new Client("localhost", 1333);
            client.Connect();
        }

        [TestMethod]
        public async Task ListCommandTestAsync()
        {
            var answer = await client.ListCommand(path);
            Assert.AreEqual("2 TestFile.txt False TestDir True ", answer.Item1);
            server.Stop();
            client.Close();
        }

        [TestMethod]
        public async Task ListCommandListRightTestAsync()
        {
            var answer = await client.ListCommand(path);

            Assert.AreEqual("TestFile.txt", answer.Item2[0].Item1);
            Assert.IsFalse(answer.Item2[0].Item2);

            Assert.AreEqual("TestDir", answer.Item2[1].Item1);
            Assert.IsTrue(answer.Item2[1].Item2);

            server.Stop();
            client.Close();
        }

        [TestMethod]
        public async Task ListCommandNotRightPath()
        {
            var answer = await client.ListCommand(path + "/NotExist");
            Assert.AreEqual("-1", answer.Item1);
            server.Stop();
            client.Close();
        }

        [TestMethod]
        public async Task GetCommandTestGoodSize()
        {
            var (size, content, messageError) = await client.GetCommand(path + "/TestFile.txt");
            Assert.AreEqual("24", size);
            server.Stop();
            client.Close();
        }

        [TestMethod]
        public async Task GetCommandTestGoodContent()
        {
            var (size, content, messageError) = await client.GetCommand(path + "/TestFile.txt");
            Assert.AreEqual("EF-BB-BF-46-69-6C-65-20-66-6F-72-20-74-65-73-74-20-70-72-6F-6A-65-63-74", BitConverter.ToString(content));
            server.Stop();
            client.Close();
        }

        [TestMethod]
        public async Task GetCommandBadPath()
        {
            var (size, content, messageError) = await client.GetCommand(path + "/TestFileeeeeee.txt");
            Assert.AreEqual("-1", size);
            server.Stop();
            client.Close();
        }
    }
}