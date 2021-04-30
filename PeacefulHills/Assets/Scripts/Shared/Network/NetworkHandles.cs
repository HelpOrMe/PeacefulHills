using Unity.Collections;

namespace PeacefulHills.Network
{
    internal static class NetworkHandles
    {
        private static NativeQueue<int> _freeHandleIndexes = new NativeQueue<int>(Allocator.Persistent);
        private static int _handlesCount;
        
        public static int GetFreeHandleIndex()
        {
            if (!_freeHandleIndexes.IsEmpty())
            {
                return _freeHandleIndexes.Dequeue();
            }
            return _handlesCount++;
        }

        public static void FreeHandle(int handleIndex)
        {
            _freeHandleIndexes.Enqueue(handleIndex);
            _handlesCount--;
        }
    }

}