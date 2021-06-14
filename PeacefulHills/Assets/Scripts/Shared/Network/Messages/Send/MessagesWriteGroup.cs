using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(MessagesSimulationGroup))]
    public class MessagesWriteGroup : ComponentSystemGroup
    {
        
    }
    
    [UpdateInGroup(typeof(MessagesWriteGroup), OrderFirst = true)]
    public class BeginMessagesWriteBuffer : EntityCommandBufferSystem
    {
    }

    [UpdateInGroup(typeof(MessagesWriteGroup), OrderLast = true)]
    public class EndMessagesWriteBuffer : EntityCommandBufferSystem
    {
    }
}