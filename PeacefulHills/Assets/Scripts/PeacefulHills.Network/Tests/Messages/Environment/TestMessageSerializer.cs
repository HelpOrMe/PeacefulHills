using PeacefulHills.Network.Messages;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Tests
{
    public struct TestMessageSerializer : IMessageSerializer<TestMessage>
    {
        public void Serialize(ref DataStreamWriter writer, in TestMessage testMessage)
        {
            writer.WriteByte(testMessage.Value1);
            writer.WriteShort(testMessage.Value2);
            writer.WriteUInt(testMessage.Value3);
            writer.WriteULong(testMessage.Value4);
        }

        public void Deserialize(ref DataStreamReader reader, ref MessageDeserializerContext context)
        {
            NetworkMessages.Receive(context, Deserialize(ref reader));
        }

        public TestMessage Deserialize(ref DataStreamReader reader)
        {
            return new TestMessage
            {
                Value1 = reader.ReadByte(),
                Value2 = reader.ReadShort(),
                Value3 = reader.ReadUInt(),
                Value4 = reader.ReadULong(),
            };
        }
    }
}