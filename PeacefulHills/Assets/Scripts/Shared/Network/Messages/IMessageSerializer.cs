using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public interface IMessageSerializer<TMessage> where TMessage : struct, IMessage
    {
        void Write(ref TMessage message, ref DataStreamWriter writer);

        TMessage Read(ref DataStreamReader reader);
    }
}