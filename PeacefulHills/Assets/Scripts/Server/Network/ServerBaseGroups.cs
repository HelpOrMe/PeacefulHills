using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class ServerInitializationGroup : ComponentSystemGroup
    {
        
    }
    
    [UpdateInGroup(typeof(ServerInitializationGroup), OrderFirst = true)]
    [ExecuteAlways]
    public class BeginServerInitializationCommandBufferSystem : EntityCommandBufferSystem
    {
        
    }
    
    [UpdateInGroup(typeof(ServerInitializationGroup), OrderLast = true)]
    [ExecuteAlways]
    public class EndServerInitializationCommandBufferSystem : EntityCommandBufferSystem
    {
        
    }
    
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class ServerSimulationGroup : ComponentSystemGroup
    {
        
    }
    
    [UpdateInGroup(typeof(ServerSimulationGroup), OrderFirst = true)]
    [ExecuteAlways]
    public class BeginServerSimulationCommandBufferSystem : EntityCommandBufferSystem
    {
        
    }
    
    [UpdateInGroup(typeof(ServerSimulationGroup), OrderLast = true)]
    [ExecuteAlways]
    public class EndServerSimulationCommandBufferSystem : EntityCommandBufferSystem
    {
        
    }
}