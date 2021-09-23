using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public struct DriverConnection : IComponentData
    {
        public NetworkConnection Value;
    }
}