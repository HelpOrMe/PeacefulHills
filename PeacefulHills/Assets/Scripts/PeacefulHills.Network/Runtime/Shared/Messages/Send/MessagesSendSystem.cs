using PeacefulHills.Extensions;
using PeacefulHills.Network.Messages.Profiling;
using PeacefulHills.Network.Profiling;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(MessagesSimulationGroup))]
    [UpdateAfter(typeof(MessagesWriteGroup))]
    public class MessagesSendSystem : SystemBase
    {
        private EntityQuery _connectionsQuery;

        protected override void OnCreate()
        {
            _connectionsQuery = GetEntityQuery(ComponentType.ReadOnly<ConnectionWrapper>(), typeof(MessagesSendBuffer));
        }

        protected override void OnUpdate()
        {
            var registry = World.GetExtension<IMessageRegistry>();
            var network = World.GetExtension<INetwork>();

            var job = new MessagesSendJob
            {
                ConnectionHandle = GetComponentTypeHandle<ConnectionWrapper>(true),
                Messages = registry.Messages,
                MessagesBufferHandle = GetBufferTypeHandle<MessagesSendBuffer>(),
                Pipeline = network.ReliablePipeline,
                Driver = network.DriverConcurrent,
                MessagesBytesSentCounter = MessagesProfilerCounters.BytesSent,
                BytesSentCounter = NetworkProfilerCounters.BytesSent
            };

            network.DriverDependency = job.ScheduleParallel(_connectionsQuery, network.DriverDependency);
            Dependency = network.DriverDependency;
        }
    }
}