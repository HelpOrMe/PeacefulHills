using NUnit.Framework;
using PeacefulHills.Network.Messages;

namespace PeacefulHills.Network.Tests
{
    public class MessageRegistryTests
    {
        public IMessageRegistry Registry;
        
        [SetUp]
        public void SetUp()
        {
            Registry = new MessageRegistry();
        }

        [TearDown]
        public void TearDown()
        {
            Registry.Dispose();
        }
        
        [Test]
        public void RegisterMessage()
        {
            Registry.Register<TestMessage, TestMessageSerializer>();
            MessageInfo messageInfo = Registry.GetInfo<TestMessage>();
            Assert.AreEqual(messageInfo.TypeInfo.Type, typeof(TestMessage));
            Assert.NotNull(messageInfo.Deserialize);
        }
    }
}