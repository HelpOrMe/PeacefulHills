using Unity.Entities;

namespace PeacefulHills.Network.Connection
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    [UpdateAfter(typeof(BeginNetworkSimulationBuffer))]
    public class ConnectionSimulationGroup : ComponentSystemGroup
    {
    }
}