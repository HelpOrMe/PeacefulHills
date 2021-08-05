using Unity.Entities;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    [UpdateAfter(typeof(BeginNetworkSimulationBuffer))]
    public class ConnectionSimulationGroup : ComponentSystemGroup
    {
    }
}