using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;
using Unity.Profiling;

namespace PeacefulHills.Network.Receive
{
     public struct NetworkReceiveJob : IJobChunk
        {
            [ReadOnly] public BufferTypeHandle<NetworkReceiveBufferPool> ReceiveBufferPoolHandle;
            [ReadOnly] public ComponentTypeHandle<ConnectionWrapper> ConnectionsHandle;
            [NativeDisableParallelForRestriction] public BufferFromEntity<NetworkReceiveBuffer> ReceiveBufferFromEntity;
            
            public NetworkDriver.Concurrent Driver;

            public ProfilerCounterValue<int> BytesReceivedCounter;
            
            public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
            {
                BufferAccessor<NetworkReceiveBufferPool> receiveBuffersPool = chunk.GetBufferAccessor(ReceiveBufferPoolHandle);
                NativeArray<ConnectionWrapper> connections = chunk.GetNativeArray(ConnectionsHandle);

                for (int i = 0; i < chunk.Count; i++)
                {
                    ConnectionWrapper connection = connections[i];
                    DynamicBuffer<NetworkReceiveBufferPool> receiveBufferPools = receiveBuffersPool[i];
                    
                    DataStreamReader reader;
                    NetworkEvent.Type cmd;
                    
                    while ((cmd = Driver.PopEventForConnection(connection.Value, out reader)) != NetworkEvent.Type.Empty)
                    {
                        if (cmd == NetworkEvent.Type.Data)
                        {
                            Entity bufferEntity = receiveBufferPools[reader.ReadByte()].Entity;
                            DynamicBuffer<NetworkReceiveBuffer> receiveBuffer = ReceiveBufferFromEntity[bufferEntity];
                            CopyToBuffer(ref reader, receiveBuffer);
                            BytesReceivedCounter.Value += reader.Length;
                        }
                    }
                }
            }

            public unsafe void CopyToBuffer(ref DataStreamReader reader, DynamicBuffer<NetworkReceiveBuffer> receiveBuffer)
            {
                int oldLength = receiveBuffer.Length;
                int length = reader.Length - reader.GetBytesRead();
                            
                receiveBuffer.ResizeUninitialized(oldLength + length);
                reader.ReadBytes((byte*)receiveBuffer.GetUnsafePtr() + oldLength, length);
            }
        }
}