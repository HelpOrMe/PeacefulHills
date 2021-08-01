using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public interface IMessageSerializer<TMessage> where TMessage : struct, IMessage
    {
        void Serialize(in TMessage message, ref DataStreamWriter writer);

        void Deserialize(ref DataStreamReader reader, ref MessageDeserializerContext context);
    }
}