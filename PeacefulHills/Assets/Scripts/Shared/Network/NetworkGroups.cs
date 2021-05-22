using PeacefulHills.Bootstrap;
using PeacefulHills.ECS;
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
        protected override void OnCreate()
        {
            base.OnCreate();
            this.RequireExtension<INetwork>();
        }

        protected override void OnUpdate()
        {
            var network = World.GetExtension<INetwork>();
            network.LastDriverJobHandle = network.Driver.ScheduleUpdate();
            
            base.OnUpdate();
        }
    }
    
    [UpdateInGroup(typeof(NetworkSimulationGroup), OrderLast = true)]
    public class EndNetworkSimulationBuffer : EntityCommandBufferSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            this.RequireExtension<INetwork>();
        }

        protected override void OnUpdate()
        {
            var network = World.GetExtension<INetwork>();
            network.LastDriverJobHandle.Complete();

            base.OnUpdate();
        }
    }
}