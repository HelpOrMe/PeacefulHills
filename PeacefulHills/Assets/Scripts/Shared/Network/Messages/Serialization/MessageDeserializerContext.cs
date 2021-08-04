using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public struct MessageDeserializerContext
    {
        public EntityCommandBuffer.ParallelWriter CommandBuffer;
        public Entity Connection;
        public int SortKey;
    }
}