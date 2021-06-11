using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public struct MessageSendRequest : IComponentData
    {
        public Entity Connection;
    }
}