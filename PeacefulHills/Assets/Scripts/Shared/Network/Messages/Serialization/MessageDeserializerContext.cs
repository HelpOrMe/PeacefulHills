using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public struct MessageDeserializerContext
    {
        public EntityCommandBuffer.ParallelWriter CommandBuffer;
        public NetworkConnection Connection;
        public int SortIndex;
    }
}