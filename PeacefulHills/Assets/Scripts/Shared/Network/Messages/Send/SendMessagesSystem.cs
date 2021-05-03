using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    [UpdateBefore(typeof(NetworkWriteMessagesGroup))]
    public class SendMessagesSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<MessagesWritingInfo>();
        }

        protected override void OnUpdate()
        { 
            var writeDependency = GetSingleton<MessagesWritingInfo>();
            SetSingleton(new MessagesWritingInfo());

            // TODO: change 1 to active connection count
            
            var connectionIds = new NativeList<int>(1, Allocator.Temp);
            var connections = new NativeHashMap<int, MessageTarget>(1, Allocator.Temp);
            var messages = new NativeHashMap<int, NativeList<OutputMessage>>(1, Allocator.Temp);

            // JobHandle extractMessagesDeps = Entities
            //     .ForEach((ref DynamicBuffer<OutputMessage> messagesBuffer, in MessageTarget target) =>
            //     {
            //         int connectionId = target.Connection.InternalId;
            //         
            //         if (!messages.ContainsKey(connectionId))
            //         {
            //             messages[connectionId] = new NativeList<OutputMessage>(Allocator.Temp);
            //         }
            //
            //         if (!connections.ContainsKey(connectionId))
            //         {
            //             connections[connectionId] = target;
            //             connectionIds.Add(connectionId);
            //         }
            //
            //         NativeArray<OutputMessage> outputMessages = messagesBuffer.ToNativeArray(Allocator.Temp);
            //         messages[connectionId].AddRange(outputMessages);
            //         outputMessages.Dispose();
            //         messagesBuffer.Clear();
            //     })
            //     .Schedule(writeDependency.Handle);
            //
            // var sortDeps = new NativeArray<JobHandle>(messages.Capacity, Allocator.Temp);
            //
            // for (int i = 0; i < connectionIds.Length; i++)
            // {
            //     var sortMessagesJob = new SortMessagesJob
            //     {
            //         Messages = messages[connectionIds[i]]
            //     };
            //     
            //     sortMessagesJob.Schedule(connectionIds.Length, )
            // }
        }
        
        [BurstCompile]
        public struct SortMessagesJob : IJobChunk
        {
            public BufferTypeHandle<OutputMessage> OutputMessagesBuffer;
            public ComponentTypeHandle<MessageTarget> MessageTargetHandle; 
            
            // public void Execute(int index)
            // {
                // for (int i = 0; i < messages.Length - 1; i++)
                // {
                //     for (int j = i + 1; j > 0; j--)
                //     {
                //         OutputMessage message = messages[j];
                //         
                //         if (messages[j - 1].Index > message.Index)
                //         {
                //             int tempIndex = messages[j - 1].Index;
                //             messages[j - 1] = message;
                //             message.Index = tempIndex;
                //             messages[j] = message;
                //         }
                //     }
                // }
            // }

            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                BufferAccessor<OutputMessage> bufferAccessor = chunk.GetBufferAccessor(OutputMessagesBuffer);
                NativeArray<MessageTarget> messageTargets = chunk.GetNativeArray(MessageTargetHandle);

                for (int i = 0; i < bufferAccessor.Length; i++)
                {
                    DynamicBuffer<OutputMessage> messages = bufferAccessor[i];
                    
                }
            }
        }
    }
}