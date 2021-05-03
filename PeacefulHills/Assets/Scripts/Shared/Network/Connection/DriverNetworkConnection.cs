using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    public struct DriverNetworkConnection : IComponentData
    {
        public NetworkConnection Connection;
    }
}