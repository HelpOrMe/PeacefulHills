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

            while (remainingMessageCount > 0)
            {
                Entity bufferEntity = CommandBuffer.CreateEntity(index);
                CommandBuffer.AddComponent(index, bufferEntity, target);
                DynamicBuffer<OutputMessage> outputBuffer = CommandBuffer.AddBuffer<OutputMessage>(index, bufferEntity);

                int start = targetMessagesCount - remainingMessageCount;
                int length = min(32, remainingMessageCount);

                for (int i = 0; i < length; i++)
                {
                    WrittenMessage message = targetMessages[start + i];

                    outputBuffer.Add(new OutputMessage {Size = message.Size, Bytes = message.Bytes});
                }

                remainingMessageCount -= 32;
            }
        }
    }
}