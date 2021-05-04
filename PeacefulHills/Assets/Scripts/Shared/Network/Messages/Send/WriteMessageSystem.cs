using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkWriteMessagesGroup))]
    [UpdateAfter(typeof(BeginNetworkWriteMessagesBuffer))]
    public class WriteMessageSystem<TMessage, TMessageSerializer> : SystemBase
        where TMessage : unmanaged, IComponentData, IMessage
        where TMessageSerializer : unmanaged, IMessageSerializer<TMessage>
    {
        private BeginNetworkWriteMessagesBuffer _commandBufferSystem;
        private EntityQuery _messageSendRequestsQuery;
        
        private uint? _messageId;
        
        protected override void OnCreate()
        {
            _commandBufferSystem = World.GetOrCreateSystem<BeginNetworkWriteMessagesBuffer>();
            
            _messageSendRequestsQuery = GetEntityQuery(
                ComponentType.ReadOnly<SendMessageRequest>(),
                ComponentType.ReadOnly<MessageTarget>(),
                ComponentType.ReadOnly<TMessage>());

            RequireSingletonForUpdate<MessageWriteDependencies>();
        }

        protected override void OnUpdate()
        {
            INetwork network = this.GetNetworkFromSingleton();
            _messageId ??= network.Messages.GetId<TMessage>();
            
            EntityCommandBuffer commandBuffer = _commandBufferSystem.CreateCommandBuffer();
            EntityCommandBuffer.ParallelWriter commandBufferParallel = commandBuffer.AsParallelWriter();
            
            var writeMessageJob = new WriteMessageIntoBuffersJob
            {
               MessageId = _messageId.Value,
               DefaultPipeline = network.DefaultPipeline, 
               EntityHandle = GetEntityTypeHandle(),
               MessageHandle = GetComponentTypeHandle<TMessage>(true),
               PipelineHandle = GetComponentTypeHandle<MessageNetworkPipeline>(true),
               
               CommandBuffer = commandBufferParallel,
            };

            JobHandle writeMessageDeps = writeMessageJob.ScheduleParallel(_messageSendRequestsQuery);

            var dependencies = GetSingleton<MessageWriteDependencies>();
            dependencies.Handle = JobHandle.CombineDependencies(writeMessageDeps, dependencies.Handle);
            SetSingleton(dependencies);
        }
        
        [BurstCompile]
        public struct WriteMessageIntoBuffersJob : IJobChunk
        {
            public uint MessageId;

            public EntityTypeHandle EntityHandle;
            public NetworkPipeline DefaultPipeline;
            public ComponentTypeHandle<TMessage> MessageHandle;
            public ComponentTypeHandle<MessageNetworkPipeline> PipelineHandle;

            public EntityCommandBuffer.ParallelWriter CommandBuffer;
            
            public unsafe void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                NativeArray<MessageNetworkPipeline> pipelines = default;
                bool chunkWithPipeline = chunk.Has(PipelineHandle);
                
                if (chunkWithPipeline)
                {
                    pipelines = chunk.GetNativeArray(PipelineHandle);
                }
                
                NativeArray<Entity> entities = chunk.GetNativeArray(EntityHandle);
                NativeArray<TMessage> messages = chunk.GetNativeArray(MessageHandle);

                int sortKey = chunkIndex + firstEntityIndex;
                var serializer = default(TMessageSerializer);

                for (int i = 0; i < messages.Length; i++)
                {
                    TMessage message = messages[i];
                    
                    var bytes = new NativeArray<byte>(sizeof(TMessage) + 4, Allocator.TempJob); // 4 = message id size
                    var dataStreamWriter = new DataStreamWriter(bytes);
                    
                    dataStreamWriter.WriteUInt(MessageId);
                    serializer.Write(in message, ref dataStreamWriter);

                    CommandBuffer.RemoveComponent<TMessage>(sortKey, entities[i]);
                    CommandBuffer.RemoveComponent<SendMessageRequest>(sortKey, entities[i]);
                    CommandBuffer.SetComponent(sortKey, entities[i], new WrittenMessage
                    {
                        Data = bytes.GetUnsafePtr(),
                        Pipeline = chunkWithPipeline ? pipelines[i].Pipeline : DefaultPipeline
                    });
                }
            }
        }
    }
}