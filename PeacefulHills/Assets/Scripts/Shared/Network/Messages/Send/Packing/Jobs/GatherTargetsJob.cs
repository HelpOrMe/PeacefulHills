using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [BurstCompile]
    public struct GatherTargetsJob : IJobChunk
    {
        [ReadOnly] public ComponentTypeHandle<MessageTarget> TargetHandle;
        
        public NativeHashMap<int, int> TargetIndexesMap;
        public NativeList<int> TargetMessagesCount;
        public NativeList<MessageTarget> Targets;
            
        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            NativeArray<MessageTarget> chunkTargets = chunk.GetNativeArray(TargetHandle);

            foreach (MessageTarget target in chunkTargets)
            {
                int targetConnectionId = target.Connection.InternalId;

                if (!TargetIndexesMap.TryGetValue(targetConnectionId, out int messageIndex))
                {
                    TargetIndexesMap[targetConnectionId] = Targets.Length;
                    TargetMessagesCount.Add(1);
                    Targets.Add(target);
                }
                else
                {
                    TargetMessagesCount[messageIndex]++;
                }
            }
        }
    }
}