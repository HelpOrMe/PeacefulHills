using Unity.Entities;

namespace PeacefulHills.Network
{
    public struct NetworkReceiveBufferPool : IBufferElementData
    {
        public Entity Entity;
    }
}