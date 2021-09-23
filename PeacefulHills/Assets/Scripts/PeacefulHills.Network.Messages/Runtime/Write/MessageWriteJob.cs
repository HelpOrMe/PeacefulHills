using PeacefulHills.Network.Packet;
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
        [ReadOnly] public ComponentTypeHandle<MessageTarget> TargetHandle;
        [ReadOnly] [DeallocateOnJobCompletion] public NativeArray<Entity> PacketRSArray;
        [NativeDisableParallelForRestriction] public BufferFromEntity<PacketSendBuffer> MessagesBufferFromEntity;

        public MessagesScheduler<TMessage, TMessageSerializer> Scheduler;
        public EntityCommandBuffer.ParallelWriter CommandBuffer;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            NativeArray<Entity> entities = chunk.GetNativeArray(EntityHandle);
            NativeArray<MessageTarget> targets = chunk.GetNativeArray(TargetHandle);

            int sortKey = chunkIndex + firstEntityIndex;

            if (ComponentType.ReadOnly<TMessage>().IsZeroSized)
            {
                TMessage message = default;
                for (int i = 0; i < chunk.Count; i++)
                {
                    Write(entities[i], targets[i], message, sortKey);
                }
            }
            else
            {
                NativeArray<TMessage> messages = chunk.GetNativeArray(MessageHandle);
                for (int i = 0; i < chunk.Count; i++)
                {
                    Write(entities[i], targets[i], messages[i], sortKey);
                }
            }
        }

        private void Write(Entity entity, MessageTarget target, TMessage message, int sortKey)
        {
            CommandBuffer.DestroyEntity(sortKey, entity);

            if (target.Connection != Entity.Null)
            {
                if (!MessagesBufferFromEntity.HasComponent(target.Connection))
                {
                    throw new NetworkSimulationException("Unable to send message to connection without messages buffer");
                }

                DynamicBuffer<PacketSendBuffer> buffer = MessagesBufferFromEntity[target.Connection];
                Scheduler.Schedule(buffer, message);
            }
            else
            {
                for (int i = 0; i < PacketRSArray.Length; i++)
                {
                    DynamicBuffer<PacketSendBuffer> buffer = MessagesBufferFromEntity[PacketRSArray[i]];
                    Scheduler.Schedule(buffer, message);
                }
            }
        }
    }
}