using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public class Network : INetwork
    {
        public NetworkDriver Driver { get; set; }
        public NetworkDriver.Concurrent DriverConcurrent { get; set; }
        public JobHandle DriverDependency { get; set; }
    }
}