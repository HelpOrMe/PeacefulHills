using PeacefulHills.Extensions;
using PeacefulHills.Network.Messages.Profiling;
using PeacefulHills.Network.Packet;
using PeacefulHills.Network.Profiling;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(MessagesSimulationGroup))]
    [UpdateAfter(typeof(MessagesWriteGroup))]
    public class MessagesSendSystem : SystemBase
    {
        private EntityQuery _agentsQuery;

        protected override void OnCreate()
        {
            _agentsQuery = GetEntityQuery(typeof(MessagePacketAgent));
        }

        protected override void OnUpdate()
        {
            var registry = World.GetExtension<IMessageRegistry>();
            var driver = World.GetExtension<INetworkDriverInfo>();

            var job = new MessagesSendJob
            {
                ConnectionFromEntity = GetComponentDataFromEntity<DriverConnection>(true),
                ConnectionLinkHandle = GetComponentTypeHandle<ConnectionLink>(true),
                SendBufferHandle = GetBufferTypeHandle<PacketSendBuffer>(),
                Messages = registry.Messages,
                Pipeline = driver.ReliablePipeline,
                Driver = driver.Concurrent,
                MessagesBytesSentCounter = MessagesProfilerCounters.BytesSent,
                BytesSentCounter = NetworkProfilerCounters.BytesSent
            };

            driver.Dependency = job.ScheduleParallel(_agentsQuery, driver.Dependency);
            Dependency = driver.Dependency;
        }
    }
}