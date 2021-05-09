using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class WriteMessagesGroup : ComponentSystemGroup
    {
        
    }
    
    [UpdateInGroup(typeof(WriteMessagesGroup), OrderFirst = true)]
    [ExecuteAlways]
    public class BeginWriteMessagesBuffer : EntityCommandBufferSystem
    {
        
    }
    
    [UpdateInGroup(typeof(WriteMessagesGroup), OrderLast = true)]
    [ExecuteAlways]
    public class EndWriteMessagesBuffer : EntityCommandBufferSystem
    {
        
    }
}