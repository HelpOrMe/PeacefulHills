using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public class ClientNetwork : Network
    {
        public NetworkPipeline ReliablePipeline;
    }
}