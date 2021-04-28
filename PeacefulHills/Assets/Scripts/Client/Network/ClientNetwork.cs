using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public class ClientNetwork : Network
    {
        public NetworkDriver Driver { get; set; }
        public NetworkPipeline ReliablePipeline;
        public JobHandle LastDriverJobHandle { get; set; }
    }
}