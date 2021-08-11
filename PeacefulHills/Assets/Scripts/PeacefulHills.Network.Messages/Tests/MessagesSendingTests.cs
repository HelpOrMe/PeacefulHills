using System.Threading.Tasks;
using NUnit.Framework;
using PeacefulHills.Bootstrap;
using PeacefulHills.Extensions;
using PeacefulHills.Network.Profiling;
using PeacefulHills.Testing;
using Unity.Entities;

namespace PeacefulHills.Network.Messages.Tests
{
    public class MessagesSendingTests
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
        public async Task BroadcastMessage_AsEntity_ToClient()
        {
            await BroadcastMessage_FromTo("Server world", "Client world №1");
        }
        
        [TestAsync]
        [Repeat(3)]
        public async Task BroadcastMessage_AsEntity_ToServer()
        {
            await BroadcastMessage_FromTo("Client world №1", "Server world");
        }
        
        [TestAsync]
        [Repeat(3)]
        public async Task SendMessage_AsEntity_ToClient()
        {
            await SendMessage_FromTo("Server world", "Client world №1");
        }
        
        [TestAsync]
        [Repeat(3)]
        public async Task SendMessage_AsEntity_ToServer()
        {
            await SendMessage_FromTo("Client world №1", "Server world");
        }
        
        [TestAsync]
        [Repeat(3)]
        public async Task ScheduleMessage_ToClient()
        {
            await ScheduleMessage_FromTo("Server world", "Client world №1");
        }
        
        [TestAsync]
        [Repeat(3)]
        public async Task ScheduleMessage_ToServer()
        {
            await ScheduleMessage_FromTo("Client world №1", "Server world");
        }
        
        public async Task BroadcastMessage_FromTo(string fromWorld, string toWorld)
        {
            Worlds.Select(fromWorld);
            TestMessage testMessage = TestMessage.Random();
            NetworkMessages.Broadcast(Worlds.Current.EntityManager, testMessage);
            
            Worlds.Select(toWorld);
            Entity messageEntity = await Wait.For<TestMessage>();
            
            AssertEntities.Components<MessageTarget, MessageReceiveRequest>(messageEntity);
            Assert.AreEqual(messageEntity.Component<TestMessage>(), testMessage);
            Assert.AreNotEqual(messageEntity.Component<MessageTarget>().Connection, Entity.Null);
            
            messageEntity.Destroy();
        }
        
        private async Task ScheduleMessage_FromTo(string worldFrom, string worldTo)
        {
            Worlds.Select(worldFrom);
            ushort testMessageId =Worlds.Current.GetExtension<IMessageRegistry>().GetId<TestMessage>();
            var scheduler = new MessagesScheduler<TestMessage, TestMessageSerializer>(testMessageId);

            DynamicBuffer<MessagesSendBuffer> clientMessagesSendBuffer = Entities.Buffer<MessagesSendBuffer>();
            TestMessage message = TestMessage.Random();
            scheduler.Schedule(clientMessagesSendBuffer, message);
            
            Worlds.Select(worldTo);
            Entity messageEntity = await Wait.For<TestMessage>();
            
            AssertEntities.Components<MessageReceiveRequest>(messageEntity);
            Assert.AreEqual(messageEntity.Component<TestMessage>(), message);
            Assert.AreNotEqual(messageEntity.Component<MessageTarget>().Connection, Entity.Null);
            
            messageEntity.Destroy();
        }
        
        private async Task SendMessage_FromTo(string worldFrom, string worldTo)
        {
            Worlds.Select(worldFrom);
            Entity clientConnection = Entities.Component<ConnectionWrapper>();
            TestMessage testMessage = TestMessage.Random();
            NetworkMessages.Send(Worlds.Current.EntityManager, clientConnection, testMessage);

            Worlds.Select(worldTo);
            Entity messageEntity = await Wait.For<TestMessage>();
            
            AssertEntities.Components<MessageReceiveRequest>(messageEntity);
            Assert.AreEqual(messageEntity.Component<TestMessage>(), testMessage);
            Assert.AreNotEqual(messageEntity.Component<MessageTarget>().Connection, Entity.Null);

            messageEntity.Destroy();
        }
    }
}