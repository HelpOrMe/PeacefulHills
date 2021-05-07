using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    public struct NetworkConnectionWrapper : IComponentData
    {
        public NetworkConnection Connection;
    }
}