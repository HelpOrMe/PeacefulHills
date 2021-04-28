using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public struct ReceiveMessageRequest : IComponentData
    {
        public NetworkConnection Connection;
    }
    
    public struct SendMessageRequest : IComponentData
    {
        public NetworkConnection Connection;
    }
}