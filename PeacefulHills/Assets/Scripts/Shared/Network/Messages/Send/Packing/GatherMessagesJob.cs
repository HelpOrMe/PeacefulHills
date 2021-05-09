using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [BurstCompile]
    public struct GatherMessagesJob : IJobChunk
    {
        [ReadOnly] public ComponentTypeHandle<MessageTarget> TargetHandle;
        [ReadOnly] public ComponentTypeHandle<WrittenMessage> MessageHandle;
        [ReadOnly] public NativeHashMap<int, int> TargetIndexesMap;

        [WriteOnly] public NativeArray<NativeList<WrittenMessage>> JaggedMessages;
            
        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            NativeArray<WrittenMessage> chunkMessages = chunk.GetNativeArray(MessageHandle);
            NativeArray<MessageTarget> chunkTargets = chunk.GetNativeArray(TargetHandle);
                
            for (int i = 0; i < chunkMessages.Length; i++)
            {
                int messageIndex = TargetIndexesMap[chunkTargets[i].Connection.InternalId];
                JaggedMessages[messageIndex].Add(chunkMessages[i]);
            }
        }
    }
}