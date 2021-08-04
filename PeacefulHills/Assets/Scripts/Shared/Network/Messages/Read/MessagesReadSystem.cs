using PeacefulHills.ECS.World;
using PeacefulHills.Network.Receive;
using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(MessagesSimulationGroup))]
    public class MessagesReadSystem : SystemBase
    {
        private EndMessagesSimulationBuffer _buffer;
        private EntityQuery _connectionsQuery;

        protected override void OnCreate()
        {
            _buffer = World.GetOrCreateSystem<EndMessagesSimulationBuffer>();
            _connectionsQuery = GetEntityQuery(
                ComponentType.ReadOnly<ConnectionWrapper>(),
                ComponentType.ReadOnly<NetworkReceiveBufferPool>());
        }

        protected override void OnUpdate()
        {
            var registry = World.GetExtension<IMessagesRegistry>();

            NativeList<MessageInfo> messages = registry.Messages;
            EntityCommandBuffer.ParallelWriter commandBuffer = _buffer.CreateCommandBuffer().AsParallelWriter();

            var job = new MessageReadJob
            {
                EntityHandle = GetEntityTypeHandle(),
                PoolHandle = GetBufferTypeHandle<NetworkReceiveBufferPool>(true),
                ReceiveBufferFromEntity = GetBufferFromEntity<NetworkReceiveBuffer>(),
                Messages = messages,
                CommandBuffer = commandBuffer
            };

            Dependency = job.ScheduleParallel(_connectionsQuery, dependsOn: Dependency);
            _buffer.AddJobHandleForProducer(Dependency);
        }
    }
}