using PeacefulHills.Network.Connection;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(MessagesSimulationGroup))]
    public class WriteMessagesGroup : ComponentSystemGroup
    {
        
    }
    
    [UpdateInGroup(typeof(WriteMessagesGroup), OrderFirst = true)]
    public class BeginWriteMessagesBuffer : EntityCommandBufferSystem
    {
    }

    [UpdateInGroup(typeof(WriteMessagesGroup), OrderLast = true)]
    public class EndWriteMessagesBuffer : EntityCommandBufferSystem
    {
    }
}