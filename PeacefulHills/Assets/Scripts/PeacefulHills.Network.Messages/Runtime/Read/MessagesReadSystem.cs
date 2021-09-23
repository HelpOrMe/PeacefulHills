using PeacefulHills.Extensions;
using PeacefulHills.Network.Packet;
using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(MessagesSimulationGroup))]
    public class MessagesReadSystem : SystemBase
    {
        private EndMessagesSimulationBuffer _buffer;
        private EntityQuery _agentsQuery;

        protected override void OnCreate()
        {
            _buffer = World.GetOrCreateSystem<EndMessagesSimulationBuffer>();
            _agentsQuery = GetEntityQuery(typeof(MessagePacketAgent));
        }

        protected override void OnUpdate()
        {
            var registry = World.GetExtension<IMessageRegistry>();

            NativeList<MessageInfo> messages = registry.Messages;
            EntityCommandBuffer.ParallelWriter commandBuffer = _buffer.CreateCommandBuffer().AsParallelWriter();

            var job = new MessageReadJob
            {
                ReceiveBufferHandle = GetBufferTypeHandle<PacketReceiveBuffer>(),
                ConnectionLinkHandle = GetComponentTypeHandle<ConnectionLink>(),
                Messages = messages,
                CommandBuffer = commandBuffer
            };

            Dependency = job.ScheduleParallel(_agentsQuery, dependsOn: Dependency);
            _buffer.AddJobHandleForProducer(Dependency);
        }
    }
}