using PeacefulHills.Network.Messages;
using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public class Network : INetwork
    {
        public NetworkDriver Driver { get; set; }
        public NetworkPipeline DefaultPipeline { get; set; }
        public JobHandle LastDriverJobHandle { get; set; }
        
        public IMessagesRegistry Messages { get; set; }
    }
}
