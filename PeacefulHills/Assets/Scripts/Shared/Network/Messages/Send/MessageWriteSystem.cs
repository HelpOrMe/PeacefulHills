using PeacefulHills.ECS.World;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Networking.Transport;

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
                Scheduler = Scheduler,
                MessagesBufferFromEntity = GetBufferFromEntity<MessagesSendBuffer>(true),
                CommandBuffer = Buffer.CreateCommandBuffer().AsParallelWriter()
            };
        }

        protected void ScheduleWriteJob<TJob>(TJob job) where TJob : unmanaged, IJobChunk
        {
            Dependency = job.ScheduleParallel(MessagesQuery, Dependency);
            Buffer.AddJobHandleForProducer(Dependency);
        }
    }
}