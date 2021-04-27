using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public interface IMessage
    {
        void Write(ref DataStreamWriter writer);
        void Read(ref DataStreamReader reader);
    }
}