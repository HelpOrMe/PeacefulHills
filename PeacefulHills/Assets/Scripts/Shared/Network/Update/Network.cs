using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public class Network
    {
        public NetworkDriver Driver;
        public JobHandle LastDriverJobHandle;
    }
}