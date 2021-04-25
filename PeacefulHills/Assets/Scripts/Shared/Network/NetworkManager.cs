using System;
using System.Collections.Generic;
using Unity.Collections;

namespace PeacefulHills.Network
{
    public static class NetworkManager
    {
        private static List<Network> _networks = new List<Network>();
        private static NativeQueue<int> _freeHandleIndexes = new NativeQueue<int>(Allocator.Persistent);

        public static NetworkHandle AddNetwork(Network network)
        {
            int freeHandleIndex = GetFreeHandleIndex();
            _networks.Insert(freeHandleIndex, network);
            return new NetworkHandle {Index = freeHandleIndex};
        }

        private static int GetFreeHandleIndex()
        {
            if (!_freeHandleIndexes.IsEmpty())
            {
                return _freeHandleIndexes.Dequeue();
            }
            return _networks.Count;
        }
        
        public static Network GetNetwork(NetworkHandle handle)
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
            _freeHandleIndexes.Enqueue(handle.Index);
            _networks.RemoveAt(handle.Index);
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