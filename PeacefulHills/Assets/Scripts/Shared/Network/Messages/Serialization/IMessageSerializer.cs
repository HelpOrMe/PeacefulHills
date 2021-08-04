using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public interface IMessageSerializer<TMessage> where TMessage : unmanaged, IMessage
    {
        void Serialize(ref DataStreamWriter writer, in TMessage message);

        void Deserialize(ref DataStreamReader reader, ref MessageDeserializerContext context);
    }
}