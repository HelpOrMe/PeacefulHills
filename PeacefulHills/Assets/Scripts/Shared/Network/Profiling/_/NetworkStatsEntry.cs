using System;
using Unity.Collections;

namespace PeacefulHills.Network.Profiling
{
    public unsafe struct NetworkStatsEntry : IDisposable
    {
        private NativeList<byte> _bytes;
        
        public NetworkStatsEntry(int capacity)
        {
            _bytes = new NativeList<byte>(capacity, Allocator.Persistent);
        }

        public void Write<T>(T value) where T : unmanaged
        {
            WriteBytes((byte*)&value, sizeof(T));
        }
        
        public void WriteBytes(byte* data, int length)
        {
            if (_bytes.Length + length > _bytes.Capacity)
            {
                _bytes.ResizeUninitialized(_bytes.Capacity * 2);
            }
            _bytes.AddRange(data, length);
        }
        
        public void Clear()
        {
            _bytes.Clear();
        }
        
        public void Dispose()
        {
            _bytes.Dispose();
        }
        
        public NativeArray<byte> AsNativeArray()
        {
            return _bytes;
        }
    }
}