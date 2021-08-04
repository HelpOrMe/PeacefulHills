using PeacefulHills.ECS.World;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(MessagesWriteGroup))]
    public abstract class WriteMessageSystem<TMessage, TMessageSerializer> : SystemBase
        where TMessage : unmanaged, IComponentData, IMessage
        where TMessageSerializer : unmanaged, IMessageSerializer<TMessage>
    {
        protected EntityQuery MessagesQuery;
        protected EntityQuery ConnectionsQuery;

        protected EndMessagesWriteBuffer Buffer;
        protected MessagesScheduler<TMessage, TMessageSerializer> Scheduler;

        protected override void OnCreate()
        {
            MessagesQuery = GetEntityQuery(
                ComponentType.ReadOnly<TMessage>(),
                ComponentType.ReadOnly<MessageTarget>(),
                ComponentType.ReadOnly<MessageSendRequest>());

            ConnectionsQuery = GetEntityQuery(
                ComponentType.ReadOnly<ConnectionWrapper>()
                
                #if !UNITY_SERVER || UNITY_EDITOR
                , ComponentType.Exclude<HostConnection>()
                #endif
            );

            // RequireForUpdate overwrites cached requirements of system,
            // so system will only update when MessagesQuery has matches.
            RequireForUpdate(MessagesQuery);

            Buffer = World.GetOrCreateSystem<EndMessagesWriteBuffer>();
            World.RequestExtension<IMessagesRegistry>(CreateScheduler);
        }

        private void CreateScheduler(IMessagesRegistry registry)
        {
            Scheduler = new MessagesScheduler<TMessage, TMessageSerializer>(registry.GetId<TMessage>());
        }

        /// <summary>
        ///     Provides a write job for inherited message systems.
        /// </summary>
        protected WriteMessageJob<TMessage, TMessageSerializer> GetWriteJob()
        {
            var connections = ConnectionsQuery.ToEntityArrayAsync(Allocator.TempJob, out JobHandle dependency);
            Dependency = JobHandle.CombineDependencies(Dependency, dependency);

            return new WriteMessageJob<TMessage, TMessageSerializer>
            {
                EntityHandle = GetEntityTypeHandle(),
                MessageHandle = GetComponentTypeHandle<TMessage>(true),
                TargetHandle = GetComponentTypeHandle<MessageTarget>(true),
                Connections = connections,
                MessagesBufferFromEntity = GetBufferFromEntity<MessagesSendBuffer>(),
                Scheduler = Scheduler,
                CommandBuffer = Buffer.CreateCommandBuffer().AsParallelWriter()
            };
        }

        /// <summary>
        ///     Handle dependency of a write job from inherited message systems.
        /// </summary>
        protected void HandleDependency(JobHandle dependency)
        {
            Dependency = dependency;
            Buffer.AddJobHandleForProducer(Dependency);
        }
    }
}