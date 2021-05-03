using Unity.Entities;

namespace PeacefulHills.Network.Connection
{
    [WriteGroup(typeof(DriverNetworkConnection))]
    public struct IgnoredConnection : IComponentData
    {
        
    }
}
