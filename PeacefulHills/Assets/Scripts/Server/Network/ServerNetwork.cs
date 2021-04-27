using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public class ServerNetwork : Network
    {
        public NetworkPipelines Pipelines;
        
        public struct NetworkPipelines
        {
            public NetworkPipeline Reliable;
            public NetworkPipeline Unreliable;
            public NetworkPipeline UnreliableFragmented;
        }
    }
}