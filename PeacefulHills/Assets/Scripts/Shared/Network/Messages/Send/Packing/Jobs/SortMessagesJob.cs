using PeacefulHills.ECS.Collections;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace PeacefulHills.Network.Messages
{
    [BurstCompile]
    public struct SortMessagesJob : IJobParallelFor
    {
        [NativeDisableParallelForRestriction] public NativeJaggedArray<WrittenMessage> JaggedMessages;

        public void Execute(int index)
        {
            NativeArray<WrittenMessage> messages = JaggedMessages[index];
            QuickSort(messages, 0, messages.Length - 1);
        }

        public void QuickSort(NativeArray<WrittenMessage> messages, int init, int end)
        {
            if (init < end)
            {
                int pivot = Partition(messages, init, end);
                QuickSort(messages, init, pivot - 1);
                QuickSort(messages, pivot + 1, end);
            }
        }

        private int Partition(NativeArray<WrittenMessage> messages, int init, int end)
        {
            WrittenMessage last = messages[end];

            int i = init - 1;
            for (int j = init; j < end; j++)
            {
                if (messages[j].Index <= last.Index)
                {
                    i++;
                    Exchange(messages, i, j);
                }
            }

            Exchange(messages, i + 1, end);
            return i + 1;
        }

        private void Exchange(NativeArray<WrittenMessage> messages, int i, int j)
        {
            WrittenMessage temp = messages[i];
            messages[i] = messages[j];
            messages[j] = temp;
        }
    }
}