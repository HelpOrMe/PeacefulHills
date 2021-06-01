using PeacefulHills.ECS.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using static Unity.Mathematics.math;

namespace PeacefulHills.Network.Messages
{
    [BurstCompile]
    public struct PackMessagesJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<MessageTarget> Targets;
        [ReadOnly] public NativeJaggedArray<WrittenMessage> TargetsMessages;
        
        public EntityCommandBuffer.ParallelWriter CommandBuffer;
        
        public unsafe void Execute(int index)
        {
            MessageTarget target = Targets[index];
            NativeArray<WrittenMessage> targetMessages = TargetsMessages[index];

            int targetMessagesCount = targetMessages.Length;
            int remainingMessageCount = targetMessagesCount;
            int priority = 0;
            
            while (remainingMessageCount > 0)
            {
                Entity bufferEntity = CommandBuffer.CreateEntity(index);
                CommandBuffer.AddComponent(index, bufferEntity, target);
                CommandBuffer.AddComponent(index, bufferEntity, new OutputMessagesPriority { Priority = priority});
                DynamicBuffer<OutputMessage> outputBuffer = CommandBuffer.AddBuffer<OutputMessage>(index, bufferEntity);

                int start = targetMessagesCount - remainingMessageCount;
                int length = min(32, remainingMessageCount);

                for (int i = 0; i < length; i++)
                {
                    outputBuffer.Add(new OutputMessage
                    {
                        Bytes = targetMessages[start + i].Bytes
                    });
                }
                remainingMessageCount -= 32;
                priority++;
            }
        }
    }
}