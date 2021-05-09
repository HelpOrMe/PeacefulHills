using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(WriteMessagesGroup))]
    public class WriteMessageSystem<TMessage, TMessageSerializer> : SystemBase
        where TMessage : unmanaged, IComponentData, IMessage
        where TMessageSerializer : unmanaged, IMessageSerializer<TMessage>
    {
        private EndWriteMessagesBuffer _commandBufferSystem;
        private EntityQuery _messageSendRequestQuery;
        
        private uint? _messageId;
        
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<NetworkSingleton>();
            RequireSingletonForUpdate<MessagesSendingDependency>();
            
            _commandBufferSystem = World.GetOrCreateSystem<EndWriteMessagesBuffer>();
            
            _messageSendRequestQuery = GetEntityQuery(
                ComponentType.ReadOnly<MessageSendRequest>(),
                ComponentType.ReadOnly<MessageTarget>(),
                ComponentType.ReadOnly<TMessage>());
        }

        protected override void OnStartRunning()
        {
            INetwork network = this.GetNetworkFromSingleton();
            _messageId ??= network.Messages.GetId<TMessage>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer commandBuffer = _commandBufferSystem.CreateCommandBuffer();
            EntityCommandBuffer.ParallelWriter commandBufferParallel = commandBuffer.AsParallelWriter();
            
            var writeMessageJob = new WriteMessageJob
            {
               MessageId = _messageId!.Value,
               EntityHandle = GetEntityTypeHandle(),
               MessageHandle = GetComponentTypeHandle<TMessage>(true),
               
               CommandBuffer = commandBufferParallel,
            };

            JobHandle writeMessageDeps = writeMessageJob.ScheduleParallel(_messageSendRequestQuery);

            var dependency = GetSingleton<MessagesSendingDependency>();
            dependency.Handle = JobHandle.CombineDependencies(writeMessageDeps, dependency.Handle);
            SetSingleton(dependency);
        }
        
        [BurstCompile]
        public struct WriteMessageJob : IJobChunk
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
                    var bytes = new NativeArray<byte>(sizeof(TMessage) + messageIdSize, Allocator.TempJob);
                    var dataStreamWriter = new DataStreamWriter(bytes);
                 
                    TMessage message = messages[i];
                    
                    dataStreamWriter.WriteUInt(MessageId);
                    serializer.Write(in message, ref dataStreamWriter);

                    Entity entity = entities[i];
                    
                    CommandBuffer.RemoveComponent<TMessage>(sortKey, entity);
                    CommandBuffer.SetComponent(sortKey, entity, new WrittenMessage
                    {
                        Index = entity.Index,
                        Bytes = (byte*)bytes.GetUnsafePtr(),
                    });
                }
            }
        }
    }
}