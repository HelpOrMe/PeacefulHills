using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public struct MessagesSendBuffer : IBufferElementData
    {
        public byte Value;
    }
}