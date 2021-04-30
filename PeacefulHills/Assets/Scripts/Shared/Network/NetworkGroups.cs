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
    [ExecuteAlways]
    public class BeginNetworkSimulationBuffer : EntityCommandBufferSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<NetworkSingleton>();
        }

        protected override void OnUpdate()
        {
            Network network = this.GetNetworkFromSingleton();
            network.LastDriverJobHandle = network.Driver.ScheduleUpdate();
            
            base.OnUpdate();
        }
    }
    
    [UpdateInGroup(typeof(NetworkSimulationGroup), OrderLast = true)]
    [ExecuteAlways]
    public class EndNetworkSimulationBuffer : EntityCommandBufferSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<NetworkSingleton>();
        }

        protected override void OnUpdate()
        {
            Network network = this.GetNetworkFromSingleton();
            network.LastDriverJobHandle.Complete();

            base.OnUpdate();
        }
    }
}