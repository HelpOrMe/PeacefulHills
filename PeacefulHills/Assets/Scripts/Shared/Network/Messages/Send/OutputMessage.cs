using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    [InternalBufferCapacity(64)]
    public unsafe struct OutputMessage : IBufferElementData
    {
        public int Index;
        public void* Data;
        public NetworkPipeline Pipeline;
    }
}