using Unity.Entities;

namespace PeacefulHills.Network.Receive
{
    public struct NetworkReceiveBuffer : IBufferElementData
    {
        public byte Value;
    }
}