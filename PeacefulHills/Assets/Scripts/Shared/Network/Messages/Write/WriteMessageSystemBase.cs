using PeacefulHills.Network.Connection;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;
using Unity.Jobs;
using UnityEngine.UIElements;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkWriteMessagesGroup))]
    public class WriteMessageSystemBase<TMessage, TMessageSerializer> : SystemBase
        where TMessage : struct, IComponentData, IMessage
        where TMessageSerializer : struct, IMessageSerializer<TMessage>
    {
        private EntityQuery _connectionsQuery;
        private EntityQuery _messagesQuery;

        protected override void OnCreate()
        {
            _connectionsQuery = GetEntityQuery(ComponentType.ReadOnly<NetworkStreamConnection>());
            
            _messagesQuery = GetEntityQuery(
                ComponentType.ReadOnly<SendMessageRequest>(), 
                typeof(TMessage));
            
            RequireSingletonForUpdate<NetworkSingleton>();
        }

        [BurstCompile]
        public struct WriteJob : IJobChunk
        {
            public ComponentTypeHandle<TMessage> MessageHandle;
            public ComponentTypeHandle<SendMessageRequest> SendRequestHandle;
            
            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                NativeArray<SendMessageRequest> requests = chunk.GetNativeArray(SendRequestHandle);
                NativeArray<TMessage> messages = chunk.GetNativeArray(MessageHandle);
            }
        }
        
        protected override void OnUpdate() 
        {
            NativeArray<Entity> connections = _connectionsQuery.ToEntityArrayAsync(Allocator.TempJob, 
                out JobHandle connectionHandle);
            
            var job = new WriteJob
            {
                MessageHandle = GetComponentTypeHandle<TMessage>(),
                SendRequestHandle = GetComponentTypeHandle<SendMessageRequest>(),
            };
            
            // TODO: Create write deps
            // Dependency = job.ScheduleParallel(_messagesQuery, writeDependency);
        }
    }
}