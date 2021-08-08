using PeacefulHills.Network.Messages;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Tests
{
    public struct TestMessage : IMessage, IComponentData
    {
        public int Value;
    }
        
    public struct TestMessageSerializer : IMessageSerializer<TestMessage>
    {
        public void Serialize(ref DataStreamWriter writer, in TestMessage testMessage)
        {
            writer.WriteInt(testMessage.Value);
        }

        public void Deserialize(ref DataStreamReader reader, ref MessageDeserializerContext context)
        {
            NetworkMessages.Receive(context, new TestMessage
            {
                Value = reader.ReadInt()
            });
        }
    }
}