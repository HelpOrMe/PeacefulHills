using PeacefulHills.ECS.World;
using PeacefulHills.Network.Connection;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(WriteMessagesGroup))]
    public abstract class WriteMessageSystem<TMessage, TMessageSerializer> : SystemBase
        where TMessage : unmanaged, IComponentData, IMessage
        where TMessageSerializer : unmanaged, IMessageSerializer<TMessage>
    {
        protected EntityQuery MessagesQuery;
        protected EntityQuery ConnectionsQuery;
        protected EndWriteMessagesBuffer Buffer;
     
        protected MessagesScheduler<TMessage, TMessageSerializer> Scheduler;
        
        protected override void OnCreate()
        {
            this.RequireExtension<IMessagesRegistry>();
            World.RequestExtension<IMessagesRegistry>(CreateScheduler);

            MessagesQuery = GetEntityQuery(typeof(TMessage), typeof(MessageSendRequest));
            ConnectionsQuery = GetEntityQuery(typeof(ConnectionWrapper));
            Buffer = World.GetOrCreateSystem<EndWriteMessagesBuffer>();
        }

        private void CreateScheduler(IMessagesRegistry registry)
        {
            Scheduler = new MessagesScheduler<TMessage, TMessageSerializer>(registry.GetId<TMessage>());
        }

        protected WriteMessageJob<TMessage, TMessageSerializer> GetWriteJob()
        {
            var connections = ConnectionsQuery.ToEntityArrayAsync(Allocator.TempJob, out JobHandle dependency);
            Dependency = JobHandle.CombineDependencies(Dependency, dependency);
            
            return new WriteMessageJob<TMessage, TMessageSerializer> 
            {
                EntityHandle = GetEntityTypeHandle(),
                MessageHandle = GetComponentTypeHandle<TMessage>(true),
                RequestHandle = GetComponentTypeHandle<MessageSendRequest>(true),
                Connections = connections,
                MessagesBufferFromEntity = GetBufferFromEntity<MessagesSendBuffer>(),
                Scheduler = Scheduler,
                CommandBuffer = Buffer.CreateCommandBuffer().AsParallelWriter()
            };
        }

        protected void HandleDependency(JobHandle dependency)
        {
            Dependency = dependency;
            Buffer.AddJobHandleForProducer(Dependency);
        }
    }
}