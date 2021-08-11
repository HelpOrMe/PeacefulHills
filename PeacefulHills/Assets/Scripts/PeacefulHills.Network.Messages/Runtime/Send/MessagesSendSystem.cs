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
            var driver = World.GetExtension<INetworkDriverInfo>();

            var job = new MessagesSendJob
            {
                ConnectionHandle = GetComponentTypeHandle<ConnectionWrapper>(true),
                Messages = registry.Messages,
                MessagesBufferHandle = GetBufferTypeHandle<MessagesSendBuffer>(),
                Pipeline = driver.ReliablePipeline,
                Driver = driver.Concurrent,
                MessagesBytesSentCounter = MessagesProfilerCounters.BytesSent,
                BytesSentCounter = NetworkProfilerCounters.BytesSent
            };

            driver.Dependency = job.ScheduleParallel(_connectionsQuery, driver.Dependency);
            Dependency = driver.Dependency;
        }
    }
}