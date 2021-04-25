using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public class Network
    {
        public NetworkDriver Driver;
        public NetworkPipelines Pipelines;

        public JobHandle LastDriverJobHandle;
        
        public struct NetworkPipelines
        {
            public NetworkPipeline Unreliable;
            public NetworkPipeline Reliable;
            public NetworkPipeline UnreliableFragmented;
        }
    }
}