using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
     [BurstCompile]
    public struct WriteMessageJob<TMessage, TMessageSerializer> : IJobChunk
        where TMessage : unmanaged, IComponentData, IMessage
        where TMessageSerializer : unmanaged, IMessageSerializer<TMessage>
    {
        [ReadOnly] public EntityTypeHandle EntityHandle;
        [ReadOnly] public ComponentTypeHandle<TMessage> MessageHandle;
        [ReadOnly] public ComponentTypeHandle<MessageSendRequest> RequestHandle;
        [ReadOnly, DeallocateOnJobCompletion] public NativeArray<Entity> Connections;
        [NativeDisableParallelForRestriction] public BufferFromEntity<MessagesSendBuffer> MessagesBufferFromEntity;
        
        public MessagesScheduler<TMessage, TMessageSerializer> Scheduler;
        public EntityCommandBuffer.ParallelWriter CommandBuffer;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            NativeArray<Entity> entities = chunk.GetNativeArray(EntityHandle);
            NativeArray<MessageSendRequest> requests = chunk.GetNativeArray(RequestHandle);

            int sortKey = chunkIndex + firstEntityIndex;

            if (ComponentType.ReadOnly<TMessage>().IsZeroSized)
            {
                TMessage message = default;
                for (int i = 0; i < chunk.Count; i++)
                {
                    Write(entities[i], requests[i], message, sortKey);
                }
            }
            else
            {
                NativeArray<TMessage> messages = chunk.GetNativeArray(MessageHandle);
                for (int i = 0; i < chunk.Count; i++)
                {
                    Write(entities[i], requests[i], messages[i], sortKey);
                }
            }
        }

        public void Write(Entity entity, MessageSendRequest request, TMessage message, int sortKey)
        {
            CommandBuffer.DestroyEntity(sortKey, entity);
            
            if (request.Connection != Entity.Null)
            {
                if (!MessagesBufferFromEntity.HasComponent(request.Connection))
                {
                    throw new NetworkSimulationException("Unable to send message to connection without messages buffer");
                }
                
                DynamicBuffer<MessagesSendBuffer> buffer = MessagesBufferFromEntity[request.Connection];
                Scheduler.Schedule(buffer, message);
            }
            else
            {
                for (int i = 0; i < Connections.Length; i++)
                {
                    DynamicBuffer<MessagesSendBuffer> buffer = MessagesBufferFromEntity[Connections[i]];
                    Scheduler.Schedule(buffer, message);
                }
            }
        }
    }
}