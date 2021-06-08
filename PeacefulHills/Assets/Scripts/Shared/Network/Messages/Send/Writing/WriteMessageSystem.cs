using PeacefulHills.ECS;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(WriteMessagesGroup))]
    public abstract class WriteMessageSystem<TMessage, TMessageSerializer> : SystemBase
        where TMessage : unmanaged, IComponentData, IMessage
        where TMessageSerializer : unmanaged, IMessageSerializer<TMessage>
    {
        protected EndWriteMessagesBuffer CommandBufferSystem;
        protected EntityQuery MessageSendRequestQuery;
        
        protected uint MessageId;
        
        protected override void OnCreate()
        {
            CommandBufferSystem = World.GetOrCreateSystem<EndWriteMessagesBuffer>();
            
            this.RequireExtension<IMessagesRegistry>();
            World.RequestExtension<IMessagesRegistry>(ExtractMessageId);
            
            MessageSendRequestQuery = GetEntityQuery(
                ComponentType.ReadOnly<MessageSendRequest>(),
                ComponentType.ReadOnly<MessageTarget>(),
                ComponentType.ReadWrite<TMessage>());
        }

        protected void ExtractMessageId(IMessagesRegistry registry)
        {
            MessageId = registry.GetIdByStableHash(TypeManager.GetTypeInfo<TMessage>().StableTypeHash);
        }

        protected WriteMessageJob GetWriteJob()
        {
            EntityCommandBuffer commandBuffer = CommandBufferSystem.CreateCommandBuffer();
            EntityCommandBuffer.ParallelWriter commandBufferParallel = commandBuffer.AsParallelWriter();
            
            return new WriteMessageJob
            {
                MessageId = MessageId,
                EntityHandle = GetEntityTypeHandle(),
                MessageHandle = GetComponentTypeHandle<TMessage>(true),
               
                CommandBuffer = commandBufferParallel,
            };
        }
        
        protected void HandleDependency()
        {
            CommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
        
        [BurstCompile]
        protected struct WriteMessageJob : IJobChunk
        {
            public uint MessageId;

            [ReadOnly] public EntityTypeHandle EntityHandle;
            [ReadOnly] public ComponentTypeHandle<TMessage> MessageHandle;

            public EntityCommandBuffer.ParallelWriter CommandBuffer;
            
            public unsafe void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                NativeArray<Entity> entities = chunk.GetNativeArray(EntityHandle);
                NativeArray<TMessage> messages = chunk.GetNativeArray(MessageHandle);

                int sortKey = chunkIndex + firstEntityIndex;
                var serializer = default(TMessageSerializer);
                
                for (int i = 0; i < messages.Length; i++)
                {
                    const int messageIdSize = 4;
                    var bytes = new NativeArray<byte>(sizeof(TMessage) + messageIdSize, Allocator.Temp);
                    var writer = new DataStreamWriter(bytes);
                 
                    TMessage message = messages[i];
                    
                    writer.WriteUInt(MessageId);
                    serializer.Write(in message, ref writer);

                    Entity entity = entities[i];
                    
                    CommandBuffer.RemoveComponent<TMessage>(sortKey, entity);
                    CommandBuffer.AddComponent(sortKey, entity, ComponentType.ReadWrite<WrittenMessage>());
                    CommandBuffer.SetComponent(sortKey, entity, new WrittenMessage
                    {
                        Index = entity.Index,
                        Size = bytes.Length,
                        Bytes = (byte*)bytes.GetUnsafePtr(),
                    });
                }
            }
        }
    }
}