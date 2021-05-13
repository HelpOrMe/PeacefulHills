using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public interface INetwork
    {
        public NetworkDriver Driver { get; }
        public NetworkPipeline DefaultPipeline { get; }
        public JobHandle LastDriverJobHandle { get; set; }
    }
}