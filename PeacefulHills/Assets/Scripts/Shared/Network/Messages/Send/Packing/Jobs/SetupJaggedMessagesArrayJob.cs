using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace PeacefulHills.Network.Messages
{
    [BurstCompile]
    public struct SetupJaggedMessagesArrayJob : IJob
    {
        [ReadOnly] public NativeList<int> TargetMessagesCount;
        [WriteOnly] public NativeArray<NativeList<WrittenMessage>> JaggedMessages;
            
        public void Execute()
        {
            for (int i = 0; i < JaggedMessages.Length; i++)
            {
                JaggedMessages[i] = new NativeList<WrittenMessage>(TargetMessagesCount[i], Allocator.TempJob);
            }
        }
    }
}