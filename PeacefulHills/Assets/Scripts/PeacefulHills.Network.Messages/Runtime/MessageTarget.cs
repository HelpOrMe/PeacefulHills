using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public struct MessageTarget : IComponentData
    {
        public Entity Connection;
    }
}