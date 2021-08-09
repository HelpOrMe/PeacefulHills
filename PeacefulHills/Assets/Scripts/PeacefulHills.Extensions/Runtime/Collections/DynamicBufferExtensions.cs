using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace PeacefulHills.Extensions
{
    public static class DynamicBufferExtensions
    {
        public static unsafe NativeArray<byte> AsBytes<T>(this DynamicBuffer<T> buffer)
            where T : struct, IBufferElementData
        {
            NativeArray<byte> array = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(
                buffer.GetUnsafePtr(), buffer.Length, Allocator.Invalid);

            #if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle safety = NativeArrayUnsafeUtility.GetAtomicSafetyHandle(buffer.AsNativeArray());
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref array, safety);
            #endif

            return array;
        }
    }
}