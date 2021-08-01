using PeacefulHills.Network.Messages.Profiling;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    public class MessagesInitializationGroup : ComponentSystemGroup
    {
    }
    
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    [UpdateAfter(typeof(ConnectionSimulationGroup))]
    public class MessagesSimulationGroup : ComponentSystemGroup
    {
    }

    [UpdateInGroup(typeof(MessagesSimulationGroup), OrderFirst = true)]
    public class BeginMessagesSimulationBuffer : EntityCommandBufferSystem
    {
        protected override void OnUpdate()
        {
            MessageProfilerCounters.BytesSent.Value = 0;
            base.OnUpdate();
        }
    }

    [UpdateInGroup(typeof(MessagesSimulationGroup), OrderLast = true)]
    public class EndMessagesSimulationBuffer : EntityCommandBufferSystem
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();
            MessageProfilerCounters.BytesSent.Sample();
        }
    }
}