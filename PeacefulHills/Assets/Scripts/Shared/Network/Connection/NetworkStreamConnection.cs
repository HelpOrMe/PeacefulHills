using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    public struct NetworkStreamConnection : IComponentData
    {
        public NetworkConnection Connection;
    }
}