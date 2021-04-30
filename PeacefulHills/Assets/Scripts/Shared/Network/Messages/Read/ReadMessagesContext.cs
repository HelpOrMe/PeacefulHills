using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public struct ReadMessagesContext : IComponentData
    {
        public DataStreamReader Reader;
    }
}