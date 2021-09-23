using PeacefulHills.Network.Packet;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;
using Unity.Profiling;

namespace PeacefulHills.Network
{
    public struct NetworkReceiveJob : IJobChunk
    {
        [ReadOnly] public BufferTypeHandle<PacketAgentsPool> PacketAgentPoolHandle;
        [ReadOnly] public ComponentTypeHandle<DriverConnection> ConnectionLinksHandle;
        [NativeDisableParallelForRestriction] public BufferFromEntity<PacketReceiveBuffer> ReceiveBufferFromEntity;

        public NetworkDriver.Concurrent Driver;
        public ProfilerCounterValue<int> BytesReceivedCounter;

        public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            BufferAccessor<PacketAgentsPool> packetAgentPools = chunk.GetBufferAccessor(PacketAgentPoolHandle);
            NativeArray<DriverConnection> connections = chunk.GetNativeArray(ConnectionLinksHandle);

            for (int i = 0; i < chunk.Count; i++)
            {
                DynamicBuffer<PacketAgentsPool> packetAgentPool = packetAgentPools[i];

                DataStreamReader reader;
                NetworkEvent.Type cmd;

                while ((cmd = Driver.PopEventForConnection(connections[i].Value, out reader)) != NetworkEvent.Type.Empty)
                {
                    if (cmd == NetworkEvent.Type.Data)
                    {
                        byte packetTypeId = reader.ReadByte();
                        if (packetAgentPool.Length >= packetTypeId) continue;
                        
                        Entity packetAgent = packetAgentPool[packetTypeId].Entity;
                        if (packetAgent == Entity.Null) continue;
                        
                        DynamicBuffer<PacketReceiveBuffer> receiveBuffer = ReceiveBufferFromEntity[packetAgent];
                        CopyToBuffer(ref reader, receiveBuffer);
                        BytesReceivedCounter.Value += reader.GetBytesRead();
                    }
                }
            }
        }

        private unsafe void CopyToBuffer(ref DataStreamReader reader, DynamicBuffer<PacketReceiveBuffer> receiveBuffer)
        {
            int oldLength = receiveBuffer.Length;
            int length = reader.Length - reader.GetBytesRead();

            receiveBuffer.ResizeUninitialized(oldLength + length);
            reader.ReadBytes((byte*) receiveBuffer.GetUnsafePtr() + oldLength, length);
        }
    }
}