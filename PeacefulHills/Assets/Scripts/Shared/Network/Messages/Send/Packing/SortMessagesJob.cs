using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;

namespace PeacefulHills.Network.Messages
{
    [BurstCompile]
    public struct SortMessagesJob : IJobParallelFor
    {
        public NativeArray<NativeList<WrittenMessage>> JaggedMessages; 
        
        public void Execute(int index)
        {
            NativeList<WrittenMessage> messages = JaggedMessages[index];
            QuickSort(ref messages, 0, messages.Length);
        }
        
        public void QuickSort(ref NativeList<WrittenMessage> messages, int init, int end)
        {
            if (init < end)
            {
                int pivot = Partition(ref messages, init, end);
                QuickSort(ref messages, init, pivot-1);
                QuickSort(ref messages, pivot + 1, end);
            }   
        }

        private int Partition(ref NativeList<WrittenMessage> messages, int init, int end)
        {
            WrittenMessage last = messages[end];
            
            int i = init - 1;
            for (int j = init; j < end; j++)
            {
                if (messages[j].Index <= last.Index)
                {
                    i++;
                    Exchange(ref messages, i, j);     
                }
            }
            
            Exchange(ref messages, i + 1, end);
            return i + 1;
        }

        private void Exchange(ref NativeList<WrittenMessage> messages, int i, int j)
        {
            WrittenMessage temp = messages[i];
            messages[i] = messages[j];
            messages[j] = temp;
        }
    }
}