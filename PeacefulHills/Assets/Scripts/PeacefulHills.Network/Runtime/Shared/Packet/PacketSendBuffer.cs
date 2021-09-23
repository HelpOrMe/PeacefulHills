using Unity.Entities;

namespace PeacefulHills.Network.Packet
{
    public struct PacketSendBuffer : IBufferElementData
    {
        public byte Value;
    }
}