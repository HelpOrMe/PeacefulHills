using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public class NetworkDriverInfo : INetworkDriverInfo
    {
        public NetworkDriver Current { get; set; }
        public NetworkDriver.Concurrent Concurrent { get; set; }

        public NetworkPipeline ReliablePipeline { get; set; }
        public NetworkPipeline UnreliablePipeline { get; set; }

        public JobHandle Dependency { get; set; }

        public void Dispose()
        {
            Dependency.Complete();
            Current.Dispose();
        }
    }
}