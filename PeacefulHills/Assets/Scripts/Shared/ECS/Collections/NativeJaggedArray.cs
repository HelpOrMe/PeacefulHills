using System;
using System.Diagnostics;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

// ReSharper disable InconsistentNaming

namespace PeacefulHills.ECS.Collections
{
    [NativeContainerSupportsDeallocateOnJobCompletion]
    [NativeContainer]
    [DebuggerDisplay("Length = {" + nameof(Length) + "}")]
    public struct NativeJaggedArray<T> : IDisposable where T : unmanaged
    {
        public unsafe T this[int row, int col]
        {
            get
            {
                CheckElementReadAccess(row, col);
                return UnsafeUtility.ReadArrayElement<T>((void*) _buffers[row], col);
            }
            [WriteAccessRequired]
            set
            {
                CheckElementWriteAccess(row, col);
                UnsafeUtility.WriteArrayElement((void*) _buffers[row], col, value);
            }
        }
        
        public unsafe NativeArray<T> this[int row]
        {
            get
            {
                NativeArray<T> array = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>(
                    (void*) _buffers[row], _bufferLengths[row], Allocator.Invalid);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref array, m_Safety);
#endif
                return array;
            }
        }
        
        public int Length { get; private set; }
        public Allocator _allocator;

        private NativeArray<ulong> _buffers;
        private NativeArray<int> _bufferLengths;

        
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        
        // Other special field names
        private AtomicSafetyHandle m_Safety;
        [NativeSetClassTypeToNullOnSchedule]
        private DisposeSentinel m_DisposeSentinel;
        
        // ReSharper disable once StaticMemberInGenericType
        private static int _staticSafetyId;
#endif

        public NativeJaggedArray(int length, Allocator allocator)
        {
            Length = length;
            
            _allocator = allocator;
            _buffers = new NativeArray<ulong>(length, allocator);
            _bufferLengths = new NativeArray<int>(length, allocator);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSentinel.Create(out m_Safety, out m_DisposeSentinel, 1, allocator);
            InitStaticSafetyId(ref m_Safety);
#endif
        }

        public unsafe void Allocate(int row, int length)
        {
            _buffers[row] = (ulong)UnsafeUtility.Malloc(length, UnsafeUtility.AlignOf<T>(), _allocator);
            _bufferLengths[row] = length;
        }

        [BurstDiscard]
        private static void InitStaticSafetyId(ref AtomicSafetyHandle handle)
        {
            if (_staticSafetyId == 0)
                _staticSafetyId = AtomicSafetyHandle.NewStaticSafetyId<NativeArray<T>>();
            AtomicSafetyHandle.SetStaticSafetyId(ref handle, _staticSafetyId);
        }
        
        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private void CheckElementReadAccess(int row, int col)
        {
            int bufferLength = _bufferLengths[row];
            if (col < 0 || col > bufferLength)
            {
                FailOutOfRangeError(row, bufferLength);
            }
            AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private void CheckElementWriteAccess(int row, int col)
        {
            int bufferLength = _bufferLengths[row];
            if (col < 0 || col > bufferLength)
            {
                FailOutOfRangeError(row, bufferLength);
            }
            AtomicSafetyHandle.CheckWriteAndThrow(m_Safety);
        }
        
        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private void FailOutOfRangeError(int bufferLength, int col)
        {
            throw new IndexOutOfRangeException($"Index {col} is out of range of '{bufferLength}' Length.");
        }
        
        [WriteAccessRequired]
        public unsafe void Dispose()
        {
            if (_allocator == Allocator.Invalid)
            {
                throw new InvalidOperationException(
                    "The NativeArray can not be Disposed because it was not allocated with a valid allocator.");
            }

            for (int i = 0; i < Length; i++)
            {
                UnsafeUtility.Free((void*)_buffers[i], _allocator);
            }

            _buffers.Dispose();
            _bufferLengths.Dispose();

#if ENABLE_UNITY_COLLECTIONS_CHECKS
            DisposeSentinel.Dispose(ref m_Safety, ref m_DisposeSentinel);
#endif
            
            Length = 0;
            _allocator = Allocator.Invalid;
        }

        public JobHandle Dispose(JobHandle dependency)
        {
            if (_allocator == Allocator.Invalid)
            {
                throw new InvalidOperationException(
                    "The NativeArray can not be Disposed because it was not allocated with a valid allocator.");
            }

            if (_allocator > Allocator.None)
            {
                var job = new DisposeBuffersJob()
                {
                    Buffers = _buffers,
                    Allocator = _allocator
                };

                dependency = job.Schedule(dependency);
                dependency = _buffers.Dispose(dependency);
                dependency = _bufferLengths.Dispose(dependency);

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                DisposeSentinel.Clear(ref m_DisposeSentinel);
                AtomicSafetyHandle.Release(m_Safety);
#endif   
            }
            
            Length = 0;
            _allocator = Allocator.Invalid;
            
            return dependency;
        }

        struct DisposeBuffersJob : IJob
        {
            public NativeArray<ulong> Buffers;
            public Allocator Allocator;
            
            public unsafe void Execute()
            {
                foreach (ulong buffer in Buffers)
                {
                    UnsafeUtility.Free((void*)buffer, Allocator);
                }
            }
        }
    }
}