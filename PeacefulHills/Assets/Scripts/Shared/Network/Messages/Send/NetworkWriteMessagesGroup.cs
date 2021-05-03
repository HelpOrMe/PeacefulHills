using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class NetworkWriteMessagesGroup : ComponentSystemGroup
    {
        
    }
    
    [UpdateInGroup(typeof(NetworkWriteMessagesGroup), OrderFirst = true)]
    [ExecuteAlways]
    public class BeginNetworkWriteMessagesBuffer : EntityCommandBufferSystem
    {
        
    }
    
    [UpdateInGroup(typeof(NetworkWriteMessagesGroup), OrderLast = true)]
    [ExecuteAlways]
    public class EndNetworkWriteMessagesBuffer : EntityCommandBufferSystem
    {
        
    }
}