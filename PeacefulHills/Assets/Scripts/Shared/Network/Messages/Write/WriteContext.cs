using Unity.Collections;
using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public struct WriteContext
    {
        public NativeHashMap<int, DataStreamWriter> ConnectionWriters;
        public JobHandle LastWriteJobHandle;
    }
}