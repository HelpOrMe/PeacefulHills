using System;
using Unity.Collections;

namespace PeacefulHills.Network
{
    public static class NetworkManager<TNetwork> where TNetwork : struct, INetwork
    {
        private static NativeList<TNetwork> _networks = new NativeList<TNetwork>();
        
        public static NetworkHandle AddNetwork(TNetwork network)
        {
            int freeHandleIndex = NetworkHandles.GetFreeHandleIndex();
            _networks[freeHandleIndex] = network;
            return new NetworkHandle {Index = freeHandleIndex};
        }

        

        public static TNetwork GetNetwork(NetworkHandle handle) 
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
            NetworkHandles.FreeHandle(handle.Index);
            _networks.RemoveAt(handle.Index);
        }

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        private static void CheckHandle(NetworkHandle handle)
        {
            if (handle.Index >= _networks.Length)
            {
                throw new ArgumentException("Invalid handle index " + handle.Index);
            }
        }
#endif
    }
}