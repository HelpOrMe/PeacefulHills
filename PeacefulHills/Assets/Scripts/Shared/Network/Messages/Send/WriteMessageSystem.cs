using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
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
        private EntityQuery _messageOutputBuffersQuery;
        
        private uint? _messageId;
        
        protected override void OnCreate()
        {
            _commandBufferSystem = World.GetOrCreateSystem<BeginNetworkWriteMessagesBuffer>();
            
            _messageSendRequestsQuery = GetEntityQuery(
                ComponentType.ReadOnly<SendMessageRequest>(),
                ComponentType.ReadOnly<MessageTarget>(),
                ComponentType.ReadOnly<TMessage>());

            _messageOutputBuffersQuery = GetEntityQuery(
                typeof(OutputMessage),
                typeof(MessageTarget));
            
            RequireSingletonForUpdate<MessagesWritingInfo>();
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
               TargetHandle = GetComponentTypeHandle<MessageTarget>(true),
               PipelineHandle = GetComponentTypeHandle<MessageNetworkPipeline>(true),
            };

            
            
            // JobHandle w = writeMessageJob.ScheduleParallel(_messageSendRequestsQuery);
            
            var dependency = GetSingleton<MessagesWritingInfo>();
            dependency.Handle = JobHandle.CombineDependencies(dependency.Handle, w);
            SetSingleton(dependency);
        }
        
        [BurstCompile]
        public struct WriteMessageIntoBuffersJob : IJobChunk
        {
            public uint MessageId;

            public EntityTypeHandle EntityHandle;
            public NetworkPipeline DefaultPipeline;
            public ComponentTypeHandle<TMessage> MessageHandle;
            public ComponentTypeHandle<MessageTarget> TargetHandle;
            public ComponentTypeHandle<MessageNetworkPipeline> PipelineHandle;

            public NativeHashMap<int, NativeQueue<OutputMessage>> OutputMessages;
            
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
                NativeArray<MessageTarget> targets = chunk.GetNativeArray(TargetHandle);

                var serializer = default(TMessageSerializer);

                for (int i = 0; i < messages.Length; i++)
                {
                    int targetConnectionId = targets[i].Connection.InternalId;
                    
                    if (!OutputMessages.ContainsKey(targetConnectionId))
                    {
                        OutputMessages[targetConnectionId] = 
                            new NativeQueue<OutputMessage>(Allocator.TempJob);
                    }
                
                    TMessage message = messages[i];
                    
                    var bytes = new NativeArray<byte>(sizeof(TMessage) + 4, Allocator.TempJob); // 4 = message id size
                    var dataStreamWriter = new DataStreamWriter(bytes);
                    
                    dataStreamWriter.WriteUInt(MessageId);
                    serializer.Write(in message, ref dataStreamWriter);
                    
                    OutputMessages[targetConnectionId].Enqueue(new OutputMessage
                    {
                        Index = entities[i].Index,
                        Data = bytes.GetUnsafePtr(),
                        Pipeline = chunkWithPipeline ? pipelines[i].Pipeline : DefaultPipeline
                    });
                }
            }
        }

        [BurstCompile]
        public struct CreateMessageBuffersJob : IJobChunk
        {
            public NativeHashMap<int, NativeQueue<OutputMessage>> TargetMessageQueues;
            
            public EntityCommandBuffer.ParallelWriter CommandBuffer;
            public ComponentTypeHandle<MessageTarget> MessageTargetHandle;

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                int sortKey = chunkIndex + firstEntityIndex;
                
                NativeArray<MessageTarget> messageTargets = chunk.GetNativeArray(MessageTargetHandle);
                
                foreach (MessageTarget target in messageTargets)
                {
                    NativeQueue<OutputMessage> queue = TargetMessageQueues[target.Connection.InternalId];
                    
                    if (queue.Count <= 0)
                    {
                        queue.Dispose();
                        continue;
                    }
                    
                    NativeArray<OutputMessage> messages = queue.ToArray(Allocator.Temp);
                    
                    messages.Dispose();
                    queue.Dispose();
                }
            }

            // public void DistributeRemainingMessages(NativeArray<OutputMessage> messages, )
            // {
            //     for (int messageCount = messages.Length; messageCount > 0; messageCount -= 64)
            //     {
            //         Entity entity = CommandBuffer.CreateEntity(sortKey);
            //         CommandBuffer.SetComponent(sortKey, entity, target);
            //         DynamicBuffer<OutputMessage> messageBuffer = CommandBuffer.AddBuffer<OutputMessage>(
            //             sortKey, entity);
            //
            //         int start = math.max(0, messageCount - 64);
            //         int length = messageCount - start;
            //         messageBuffer.AddRange(messages.GetSubArray(start, length));
            //     }
            // }
        }
    }
}