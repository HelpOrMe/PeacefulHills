using PeacefulHills.Bootstrap;
using Unity.Entities;

namespace PeacefulHills.Network
{
    [BootstrapWorld(typeof(NetworkWorldBootstrap))]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class NetworkInitializationGroup : ComponentSystemGroup
    {
    }

    [UpdateInGroup(typeof(NetworkInitializationGroup), OrderFirst = true)]
    [AlwaysUpdateSystem]
    public class BeginNetworkInitializationBuffer : EntityCommandBufferSystem
    {
    }

    [UpdateInGroup(typeof(NetworkInitializationGroup), OrderLast = true)]
    [AlwaysUpdateSystem]
    public class EndNetworkInitializationBuffer : EntityCommandBufferSystem
    {
    }

    [BootstrapWorld(typeof(NetworkWorldBootstrap))]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class NetworkSimulationGroup : ComponentSystemGroup
    {
    }

    [UpdateInGroup(typeof(NetworkSimulationGroup), OrderFirst = true)]
    public class BeginNetworkSimulationBuffer : EntityCommandBufferSystem
    {
    }

    [UpdateInGroup(typeof(NetworkSimulationGroup), OrderLast = true)]
    public class EndNetworkSimulationBuffer : EntityCommandBufferSystem
    {
    }
}