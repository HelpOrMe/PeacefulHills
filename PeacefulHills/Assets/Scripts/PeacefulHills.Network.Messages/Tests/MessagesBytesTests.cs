using System.Threading.Tasks;
using NUnit.Framework;
using PeacefulHills.Bootstrap;
using PeacefulHills.Extensions;
using PeacefulHills.Network.Profiling;
using PeacefulHills.Testing;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages.Tests
{
    public class MessagesBytesTests
    {
        [SetUp]
        public void Setup()
        {
            Worlds.Destroy();
            WorldBootstraps.ForceInitialize<DefaultWorldBootstrap>();
            WorldBootstraps.ForceInitialize<SplitNetworkWorld>();
        }

        [TestAsync]
        [Repeat(3)]
        public async Task BroadcastMessage_SendBufferHasValidBytes()
        {
            Worlds.Select("Server world");
            Systems.Disable<MessagesSendSystem>();
        
            TestMessage testMessage = TestMessage.Random();
            NetworkMessages.Broadcast(Worlds.Current.EntityManager, testMessage);

            BufferQueryMutable<MessagesSendBuffer> sendBufferMutable = Entities.Buffer<MessagesSendBuffer>();
            await Wait.For(() => sendBufferMutable.Buffer.Length > 1);
            
            var sendBytesReader = new DataStreamReader(sendBufferMutable.Buffer.AsBytes());
            sendBytesReader.ReadByte(); // Ignore package id
            sendBytesReader.ReadUShort(); // Ignore message id

            TestMessage testMessageToSend = default(TestMessageSerializer).Deserialize(ref sendBytesReader);
            Assert.AreEqual(testMessage, testMessageToSend);
            
            sendBufferMutable.Buffer.ResizeUninitialized(1);
            Systems.Enable<MessagesSendSystem>();
        }
        
        [TestAsync]
        [Repeat(3)]
        public async Task BroadcastMessage_ReceiveBufferHasValidBytes()
        {
            Worlds.Select("Server world");
        
            TestMessage testMessage = TestMessage.Random();
            NetworkMessages.Broadcast(Worlds.Current.EntityManager, testMessage);

            Worlds.Select("Client world №1");
            Systems.Disable<MessagesReadSystem>();
            
            BufferQueryMutable<NetworkReceiveBuffer> receiveBufferMutable = Entities.Buffer<NetworkReceiveBuffer>();
            await Wait.For(() => receiveBufferMutable.Buffer.Length > 1);
            
            var receivedBytesReader = new DataStreamReader(receiveBufferMutable.Buffer.AsBytes());
            receivedBytesReader.ReadUShort(); // Ignore message id

            TestMessage testMessageToSend = default(TestMessageSerializer).Deserialize(ref receivedBytesReader);
            Assert.AreEqual(testMessage, testMessageToSend);
            
            receiveBufferMutable.Buffer.Clear();
            Systems.Enable<MessagesReadSystem>();
        }
    }
}