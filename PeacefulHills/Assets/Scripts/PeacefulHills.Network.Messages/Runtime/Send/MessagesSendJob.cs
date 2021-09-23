using PeacefulHills.Extensions;
using PeacefulHills.Network.Packet;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Networking.Transport;
using Unity.Profiling;

namespace PeacefulHills.Network.Messages
{
    [BurstCompile]
    public struct MessagesSendJob : IJobChunk
    {
        [ReadOnly] public NativeList<MessageInfo> Messages;

        [ReadOnly] public ComponentDataFromEntity<DriverConnection> ConnectionFromEntity;
        [ReadOnly] public ComponentTypeHandle<ConnectionLink> ConnectionLinkHandle;
        public BufferTypeHandle<PacketSendBuffer> SendBufferHandle;

        public NetworkPipeline Pipeline;
        public NetworkDriver.Concurrent Driver;

        public ProfilerCounterValue<int> MessagesBytesSentCounter;
        public ProfilerCounterValue<int> BytesSentCounter;

        public unsafe void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            NativeArray<ConnectionLink> connectionLinks = chunk.GetNativeArray(ConnectionLinkHandle);
            BufferAccessor<PacketSendBuffer> sendBuffers = chunk.GetBufferAccessor(SendBufferHandle);

            for (int i = 0; i < chunk.Count; i++)
            {
                DriverConnection connection = ConnectionFromEntity[connectionLinks[i].Entity];
                DynamicBuffer<PacketSendBuffer> sendBuffer = sendBuffers[i];

                while (sendBuffer.Length > 1)
                {
                    if (Driver.BeginSend(Pipeline, connection.Value, out DataStreamWriter writer) != 0)
                    {
                        break;
                    }

                    if (sendBuffer.Length <= writer.Capacity)
                    {
                        writer.WriteBytes((byte*) sendBuffer.GetUnsafeReadOnlyPtr(), 
                            sendBuffer.Length);
                    }
                    else
                    {
                        SendFitPart(ref writer, sendBuffer);
                    }

                    if (Driver.EndSend(writer) <= 0)
                    {
                        break;
                    }

                    MessagesBytesSentCounter.Value += writer.Length;
                    BytesSentCounter.Value += writer.Length;

                    if (writer.Length >= sendBuffer.Length)
                    {
                        sendBuffer.ResizeUninitialized(1);
                    }
                    else
                    {
                        // Compact the buffer
                        for (int moveIndex = writer.Length; moveIndex < sendBuffer.Length; moveIndex++)
                        {
                            sendBuffer[1 + moveIndex - writer.Length] = sendBuffer[moveIndex];
                        }

                        // Clear all except the first byte that identifies the packet as a message
                        sendBuffer.ResizeUninitialized(1 + sendBuffer.Length - writer.Length);
                    }
                }
            }
        }

        private unsafe void SendFitPart(ref DataStreamWriter writer, DynamicBuffer<PacketSendBuffer> sendBuffer)
        {
            NativeArray<byte> messageBytesArray = sendBuffer.AsBytes();

            var reader = new DataStreamReader(messageBytesArray);
            reader.ReadByte(); // Skip package type byte
            ushort messageId = reader.ReadUShort(); // Get message id size (two bytes)
            int messageSize = Messages[messageId].TypeInfo.TypeSize + 2 + 1;

            while (writer.Length + messageSize <= writer.Capacity)
            {
                writer.WriteBytes((byte*) messageBytesArray.GetUnsafeReadOnlyPtr(), messageSize);
                int bytesLeft = messageBytesArray.Length - messageSize;

                if (bytesLeft > 0)
                {
                    messageBytesArray = messageBytesArray.GetSubArray(messageSize, bytesLeft);
                    reader = new DataStreamReader(messageBytesArray);
                    messageId = reader.ReadUShort();
                    messageSize = Messages[messageId].TypeInfo.TypeSize + 2;
                }
            }

            if (writer.Length == 0)
            {
                throw new NetworkSimulationException(
                    $"Cannot send message {messageId} because its size is too big: ({messageSize}/{writer.Capacity}");
            }
        }
    }
}