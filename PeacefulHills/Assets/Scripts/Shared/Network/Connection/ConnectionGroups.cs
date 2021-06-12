using Unity.Entities;

namespace PeacefulHills.Network.Connection
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    [UpdateAfter(typeof(BeginNetworkSimulationBuffer))]
    public class ConnectionSimulationGroup : ComponentSystemGroup
    {
    }

    [UpdateInGroup(typeof(ConnectionSimulationGroup), OrderFirst = true)]
    public class BeginConnectionSimulationBuffer : EntityCommandBufferSystem
    {
    }

    [UpdateInGroup(typeof(ConnectionSimulationGroup), OrderLast = true)]
    public class EndConnectionSimulationBuffer : EntityCommandBufferSystem
    {
    }
}