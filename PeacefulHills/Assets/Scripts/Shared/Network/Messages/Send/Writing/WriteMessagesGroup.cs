using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class WriteMessagesGroup : ComponentSystemGroup
    {
        
    }
    
    [UpdateInGroup(typeof(WriteMessagesGroup), OrderFirst = true)]
    [AlwaysUpdateSystem]
    public class BeginWriteMessagesBuffer : EntityCommandBufferSystem
    {
        
    }
    
    [UpdateInGroup(typeof(WriteMessagesGroup), OrderLast = true)]
    [AlwaysUpdateSystem]
    public class EndWriteMessagesBuffer : EntityCommandBufferSystem
    {
        
    }
}