using System;
using System.Collections.Generic;
using Unity.Collections;

namespace PeacefulHills.Network
{
    public static class NetworkManager
    {
        private static List<INetwork> _networks = new List<INetwork>();
        
        private static NativeQueue<int> _freeHandleIndexes = new NativeQueue<int>(Allocator.Persistent);
        private static int _handlesCount;
        
        public static NetworkHandle AddNetwork(INetwork network)
        {
            int freeHandleIndex = GetFreeHandleIndex();
            _networks.Insert(freeHandleIndex, network);
            return new NetworkHandle {Index = freeHandleIndex};
        }
        
        public static int GetFreeHandleIndex()
        {
            if (!_freeHandleIndexes.IsEmpty())
            {
                return _freeHandleIndexes.Dequeue();
            }
            return _handlesCount++;
        }
        
        public static INetwork GetNetwork(NetworkHandle handle) 
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            CheckHandle(handle);
#endif
            return _networks[handle.Index];
        }

        public static void RemoveNetwork(NetworkHandle handle)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            CheckHandle(handle);
#endif
            FreeHandle(handle.Index);
            _networks.RemoveAt(handle.Index);
        }

        public static void FreeHandle(int handleIndex)
        {
            _freeHandleIndexes.Enqueue(handleIndex);
            _handlesCount--;
        }
        
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private static void CheckHandle(NetworkHandle handle)
        {
            if (handle.Index >= _networks.Count)
            {
                throw new ArgumentException("Invalid handle index " + handle.Index);
            }
        }
#endif
    }
}