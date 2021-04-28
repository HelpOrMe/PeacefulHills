using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public struct MessageNetworkPipeline : IComponentData
    {
        public NetworkPipeline Pipeline;
    }
}