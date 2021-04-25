using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class ClientInitializationGroup : ComponentSystemGroup
    {
        
    }
    
    [UpdateInGroup(typeof(ClientInitializationGroup), OrderFirst = true)]
    [ExecuteAlways]
    public class BeginClientInitializationCommandBufferSystem : EntityCommandBufferSystem
    {
        
    }
    
    [UpdateInGroup(typeof(ClientInitializationGroup), OrderLast = true)]
    [ExecuteAlways]
    public class EndClientInitializationCommandBufferSystem : EntityCommandBufferSystem
    {
        
    }
    
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class ClientSimulationGroup : ComponentSystemGroup
    {
        
    }
    
    [UpdateInGroup(typeof(ClientSimulationGroup), OrderFirst = true)]
    [ExecuteAlways]
    public class BeginClientSimulationCommandBufferSystem : EntityCommandBufferSystem
    {
        
    }
    
    [UpdateInGroup(typeof(ClientSimulationGroup), OrderLast = true)]
    [ExecuteAlways]
    public class EndClientSimulationCommandBufferSystem : EntityCommandBufferSystem
    {
        
    }
}