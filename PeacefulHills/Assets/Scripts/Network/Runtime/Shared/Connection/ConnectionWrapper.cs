using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public struct ConnectionWrapper : IComponentData
    {
        public NetworkConnection Value;
    }
}