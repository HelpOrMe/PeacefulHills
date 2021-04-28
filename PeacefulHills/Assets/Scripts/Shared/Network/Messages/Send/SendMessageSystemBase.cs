using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkSendMessagesGroup))]
    public class SendMessageSystemBase<TMessage, TMessageSerializer> : SystemBase 
        where TMessage : struct, IBufferElementData, IMessage
        where TMessageSerializer : struct, IMessageSerializer<TMessage>
    {
        private EntityQuery _genericQuery;

        protected override void OnCreate()
        {
            _genericQuery = GetEntityQuery(typeof(TMessage), ComponentType.ReadWrite<SendMessageRequest>());
            RequireSingletonForUpdate<NetworkSingleton>();
        }

        [BurstCompile]
        public struct SendJob : IJobChunk
        {
            public NetworkPipeline DefaultPipeline;
            public NetworkDriver.Concurrent ConcurrentDriver;
            public ComponentTypeHandle<TMessage> MessageHandle;
            public ComponentTypeHandle<SendMessageRequest> SendRequestHandle;
            public ComponentTypeHandle<MessageNetworkPipeline> PipelineHandle;
            
            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                NetworkPipeline pipeline = chunk.Has(PipelineHandle)
                    ? chunk.GetChunkComponentData(PipelineHandle).Pipeline
                    : DefaultPipeline;

                NativeArray<SendMessageRequest> requests = chunk.GetNativeArray(SendRequestHandle);
                NativeArray<TMessage> messages = chunk.GetNativeArray(MessageHandle);
            }
        } 
        
        protected override void OnUpdate()
        {
            Network network = this.GetNetworkFromSingleton();
            NetworkDriver.Concurrent concurrentDriver = this.GetNetworkFromSingleton().Driver.ToConcurrent();
            
            var job = new SendJob
            {
                DefaultPipeline = network.ReliablePipeline,
                ConcurrentDriver = concurrentDriver,
                MessageHandle = GetComponentTypeHandle<TMessage>(),
                SendRequestHandle = GetComponentTypeHandle<SendMessageRequest>(),
                PipelineHandle = GetComponentTypeHandle<MessageNetworkPipeline>()
            };

            // TODO: Create write deps
            Dependency = job.ScheduleSingle(_genericQuery, Dependency);
        }
    }
}