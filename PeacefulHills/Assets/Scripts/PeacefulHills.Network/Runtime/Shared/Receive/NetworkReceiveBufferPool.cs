using Unity.Entities;

namespace PeacefulHills.Network.Receive
{
    public struct NetworkReceiveBufferPool : IBufferElementData
    {
        public Entity Entity;
    }
}