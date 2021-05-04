using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public unsafe struct WrittenMessage : IComponentData
    {
        public void* Data;
        public NetworkPipeline Pipeline;
    }
}