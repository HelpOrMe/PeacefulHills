using Unity.Entities;

namespace PeacefulHills.Network
{
    public struct NetworkReceiveBuffer : IBufferElementData
    {
        public byte Value;
    }
}