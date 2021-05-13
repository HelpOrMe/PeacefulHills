using PeacefulHills.ECS;
using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class NetworkInitializationGroup : ComponentSystemGroup
    {
        
    }
    
    [UpdateInGroup(typeof(NetworkInitializationGroup), OrderFirst = true)]
    [ExecuteAlways]
    public class BeginNetworkInitializationBuffer : EntityCommandBufferSystem
    {
        
    }
    
    [UpdateInGroup(typeof(NetworkInitializationGroup), OrderLast = true)]
    [ExecuteAlways]
    public class EndNetworkInitializationBuffer : EntityCommandBufferSystem
    {
        
    }
    
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