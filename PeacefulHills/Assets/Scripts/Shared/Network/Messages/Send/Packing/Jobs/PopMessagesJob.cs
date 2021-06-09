using PeacefulHills.ECS.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [BurstCompile]
    public struct PopMessagesJob : IJobChunk
    {
        [ReadOnly] public ComponentTypeHandle<MessageTarget> TargetHandle;
        [ReadOnly] public ComponentTypeHandle<WrittenMessage> MessageHandle;
        [ReadOnly] public EntityTypeHandle EntityHandle;
        public EntityCommandBuffer.ParallelWriter CommandBuffer;

        [ReadOnly] public NativeHashMap<int, int> TargetIndexesMap;
        public NativeArray<int> TargetTempIndices;

        [NativeDisableContainerSafetyRestriction]
        public NativeJaggedArray<WrittenMessage> JaggedMessages;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            NativeArray<WrittenMessage> chunkMessages = chunk.GetNativeArray(MessageHandle);
            NativeArray<MessageTarget> chunkTargets = chunk.GetNativeArray(TargetHandle);
            NativeArray<Entity> chunkEntities = chunk.GetNativeArray(EntityHandle);

            for (int i = 0; i < chunkMessages.Length; i++)
            {
                CommandBuffer.DestroyEntity(chunkIndex * firstEntityIndex, chunkEntities[i]);

                int targetIndex = TargetIndexesMap[chunkTargets[i].Connection.InternalId];
                NativeArray<WrittenMessage> messages = JaggedMessages[targetIndex];
                messages[TargetTempIndices[targetIndex]++] = chunkMessages[i];
            }
        }
    }
}