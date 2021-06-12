using PeacefulHills.Network.Connection;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    [UpdateAfter(typeof(ConnectionSimulationGroup))]
    public class MessagesSimulationGroup : ComponentSystemGroup
    {
    }

    [UpdateInGroup(typeof(MessagesSimulationGroup), OrderFirst = true)]
    public class BeginMessagesSimulationBuffer : EntityCommandBufferSystem
    {
    }

    [UpdateInGroup(typeof(MessagesSimulationGroup), OrderLast = true)]
    public class EndMessagesSimulationBuffer : EntityCommandBufferSystem
    {
    }
}