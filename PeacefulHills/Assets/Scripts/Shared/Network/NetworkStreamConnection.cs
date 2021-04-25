using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public struct NetworkStreamConnection : IComponentData
    {
        public NetworkConnection Connection;
    }
}