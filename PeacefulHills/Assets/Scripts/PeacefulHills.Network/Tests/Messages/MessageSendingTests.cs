using System.Threading.Tasks;
using NUnit.Framework;
using PeacefulHills.Bootstrap;
using PeacefulHills.Network.Messages;
using PeacefulHills.Testing;
using Unity.Entities;

namespace PeacefulHills.Network.Tests
{
    public class MessageSendingTests
    {
        [SetUp]
        public void Setup()
        {
            Worlds.Destroy();
            WorldBootstraps.Initialize();
        }

        [TestAsync]
        public async Task BroadcastMessage_AsEntity()
        {
            Worlds.Select("Server world");
            NetworkMessages.Broadcast(Worlds.Now.EntityManager, new TestMessage { Value = 17 });
            
            Worlds.Select("Client world №1");
            Entity entity = await Wait.For<TestMessage>();

            AssertEntities.Components<MessageTarget, MessageReceiveRequest>(entity);
            Assert.AreEqual(entity.Component<TestMessage>().Value, 17);
            Assert.AreNotEqual(entity.Component<MessageTarget>().Connection, Entity.Null);
        }
    }
}