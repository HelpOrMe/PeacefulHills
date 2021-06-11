using Unity.Entities;

namespace PeacefulHills.Network.Connection
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class ConnectionSimulationGroup : ComponentSystemGroup
    {
    }

    [UpdateInGroup(typeof(ConnectionSimulationGroup), OrderFirst = true)]
    [AlwaysUpdateSystem]
    public class BeginConnectionSimulationBuffer : EntityCommandBufferSystem
    {
    }

    [UpdateInGroup(typeof(ConnectionSimulationGroup), OrderLast = true)]
    [AlwaysUpdateSystem]
    public class EndConnectionSimulationBuffer : EntityCommandBufferSystem
    {
    }
}