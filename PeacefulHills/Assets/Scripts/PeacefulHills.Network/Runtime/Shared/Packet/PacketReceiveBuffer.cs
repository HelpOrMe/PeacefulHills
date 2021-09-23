using Unity.Entities;

namespace PeacefulHills.Network.Packet
{
    public struct PacketReceiveBuffer : IBufferElementData
    {
        public byte Value;
    }
}