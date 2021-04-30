using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public interface INetwork
    {
        public NetworkDriver Driver { get; set; }
        public NetworkPipeline DefaultPipeline { get; set; }
        public JobHandle LastDriverJobHandle { get; set; }
    }
}