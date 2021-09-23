using PeacefulHills.Extensions;
using PeacefulHills.Network.Packet;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    [BurstCompile]
    public struct MessageReadJob : IJobEntityBatch
    {
        public BufferTypeHandle<PacketReceiveBuffer> ReceiveBufferHandle;
        [ReadOnly] public ComponentTypeHandle<ConnectionLink> ConnectionLinkHandle;

        [ReadOnly] public NativeList<MessageInfo> Messages;
        public EntityCommandBuffer.ParallelWriter CommandBuffer;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            BufferAccessor<PacketReceiveBuffer> receiveBuffers = batchInChunk.GetBufferAccessor(ReceiveBufferHandle);
            NativeArray<ConnectionLink> connectionLinks = batchInChunk.GetNativeArray(ConnectionLinkHandle);

            for (int i = 0; i < batchInChunk.Count; i++)
            {
                DynamicBuffer<PacketReceiveBuffer> receiveBuffer = receiveBuffers[i];

                var reader = new DataStreamReader(receiveBuffer.AsBytes());

                while (reader.GetBytesRead() < reader.Length)
                {
                    int messageId = reader.ReadUShort();
                    MessageInfo messageInfo = Messages[messageId];
                    var context = new MessageDeserializerContext
                    {
                        CommandBuffer = CommandBuffer,
                        Connection = connectionLinks[i].Entity,
                        SortKey = batchIndex
                    };

                    messageInfo.Deserialize.Invoke(ref reader, ref context);
                }

                receiveBuffer.Clear();
            }
        }
    }
}