using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    public struct ConnectionWrapper : IComponentData
    {
        public NetworkConnection Value;
    }
}