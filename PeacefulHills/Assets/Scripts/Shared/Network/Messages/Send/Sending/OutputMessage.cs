using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    // 32 is max window size of the reliable pipeline
    [InternalBufferCapacity(32)]
    public struct OutputMessage : IBufferElementData
    {
        public unsafe byte* Bytes;
    }
}