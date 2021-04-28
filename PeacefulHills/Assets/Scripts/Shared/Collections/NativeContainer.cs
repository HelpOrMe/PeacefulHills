using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace PeacefulHills.Collections
{
    [StructLayout(LayoutKind.Sequential)]
    [NativeContainer]
    public unsafe struct NativeContainer : IDisposable
    {
        [NativeDisableUnsafePtrRestriction]
        private void* _data;
        private Allocator _allocator;
        
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        [NativeSetClassTypeToNullOnSchedule]
        private DisposeSentinel _disposeSentinel;
        private AtomicSafetyHandle _safety;
#endif
        
        public static NativeContainer Create<T>(T value, Allocator allocator) where T : unmanaged
        {
            NativeContainer container = Allocate<T>(allocator);
            container.Write(value);
            return container;
        }

        private static NativeContainer Allocate<T>(Allocator allocator)
            where T : unmanaged
        {
            var nativeContainer = new NativeContainer
            {
                _data = UnsafeUtility.Malloc(UnsafeUtility.SizeOf<T>(), UnsafeUtility.AlignOf<T>(), allocator),
                _allocator = allocator
            };
            
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSentinel.Create(out nativeContainer._safety, out nativeContainer._disposeSentinel, 1, allocator);
#endif

            return nativeContainer;
        }

        public T Read<T>() where T : unmanaged
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(_safety);
#endif
            return *(T*)_data;
        }

        public void Write<T>(T value) where T : unmanaged
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckWriteAndThrow(_safety);
#endif
            *(T*) _data = value;
        }

        public void Dispose()
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            if (_data == null)
            {
                throw new ObjectDisposedException("The NativeReference is already disposed.");
            }
#endif
            if (_allocator > Allocator.None)
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                DisposeSentinel.Dispose(ref _safety, ref _disposeSentinel);
#endif
                UnsafeUtility.Free(_data, _allocator);
                _allocator = Allocator.Invalid;
            }

            _data = null;
        }
    }
}