using PeacefulHills.Network.Profiling;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Networking.Transport;
using UnityEngine;

namespace PeacefulHills.Network.Messages
{
    [BurstCompile]
    public struct MessagesSendJob : IJobChunk
    {
        [ReadOnly] public ComponentTypeHandle<ConnectionWrapper> ConnectionHandle;
        [ReadOnly] public NativeList<MessageInfo> Messages;

        public BufferTypeHandle<MessagesSendBuffer> MessagesBufferHandle;
        
        public NetworkPipeline Pipeline;
        public NetworkDriver.Concurrent Driver;
        
        public unsafe void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
        {
            NativeArray<ConnectionWrapper> connections = chunk.GetNativeArray(ConnectionHandle);
            BufferAccessor<MessagesSendBuffer> messageBuffers = chunk.GetBufferAccessor(MessagesBufferHandle);

            for (int i = 0; i < chunk.Count; i++)
            {
                ConnectionWrapper connectionWrapper = connections[i];
                DynamicBuffer<MessagesSendBuffer> messagesBuffer = messageBuffers[i];

                while (messagesBuffer.Length > 1)
                {
                    int result;
                    if ((result = Driver.BeginSend(Pipeline, connectionWrapper.Value, out var writer)) != 0)
                    {
                        Debug.LogWarning($"An error occured during BeginSend. ErrorCode: {result}");
                        break;
                    }
                
                    if (messagesBuffer.Length <= writer.Capacity)
                    {
                        writer.WriteBytes((byte*)messagesBuffer.GetUnsafeReadOnlyPtr(), messagesBuffer.Length);
                    }
                    else
                    {
                        SendFitPart(ref writer, messagesBuffer);
                    }
                    
                    if ((result = Driver.EndSend(writer)) <= 0)
                    {
                        Debug.LogWarning($"An error occured during EndSend. ErrorCode: {result}");
                        break;
                    }
                    
                    if (writer.Length >= messagesBuffer.Length)
                    {
                        messagesBuffer.ResizeUninitialized(1);
                    }
                    else
                    {
                        // Compact the buffer
                        for (int moveIndex = writer.Length; moveIndex < messagesBuffer.Length; moveIndex++)
                        {
                            messagesBuffer[1 + moveIndex - writer.Length] = messagesBuffer[moveIndex];
                        }
                        
                        // Clear all except the first byte that identifies the packet as a message
                        messagesBuffer.ResizeUninitialized(1 + messagesBuffer.Length - writer.Length);
                    }
                }
            }
        }

        private unsafe void SendFitPart(ref DataStreamWriter writer, DynamicBuffer<MessagesSendBuffer> messagesBuffer)
        {
            NativeArray<byte> messageBytesArray = ToArray(messagesBuffer);
                    
            var reader = new DataStreamReader(messageBytesArray);
            reader.ReadByte(); // Skip package type byte
            ushort messageId = reader.ReadUShort(); // Get message id size (two bytes)
            int messageSize = Messages[messageId].TypeInfo.TypeSize + 2 + 1;
            
            while (writer.Length + messageSize <= writer.Capacity)
            {
                writer.WriteBytes((byte*)messageBytesArray.GetUnsafeReadOnlyPtr(), messageSize);
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
                    $"Cannot send message {messageId}, because its size is too big: ({messageSize}/{writer.Capacity}");
            }
        }
        
        private unsafe NativeArray<byte> ToArray(DynamicBuffer<MessagesSendBuffer> buffer)
        {
            NativeArray<byte> sendArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(
                buffer.GetUnsafePtr(), buffer.Length, Allocator.Invalid);
            
            #if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle safety = NativeArrayUnsafeUtility.GetAtomicSafetyHandle(buffer.AsNativeArray());
                NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref sendArray, safety);
            #endif

            return sendArray;
        }
    }
}