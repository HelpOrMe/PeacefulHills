using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public readonly struct MessagesScheduler<TMessage, TMessageSerializer>
        where TMessage : unmanaged, IComponentData, IMessage
        where TMessageSerializer : unmanaged, IMessageSerializer<TMessage>
    {
        public readonly ushort MessageId;

        public MessagesScheduler(ushort messageId)
        {
            MessageId = messageId;
        }

        public unsafe void Schedule(DynamicBuffer<MessagesSendBuffer> messagesSendBuffer, TMessage message)
        {
            var serializer = default(TMessageSerializer);
            
            const int messageIdSize = 2;
            var writer = new DataStreamWriter(sizeof(TMessage) + messageIdSize, Allocator.Temp);
                
            writer.WriteUShort(MessageId);
            serializer.Serialize(in message, ref writer);

            int previousBufferLength = messagesSendBuffer.Length;
            messagesSendBuffer.ResizeUninitialized(messagesSendBuffer.Length + writer.Length);
            byte* ptr = (byte*) messagesSendBuffer.GetUnsafePtr() + previousBufferLength;
             
            UnsafeUtility.MemCpy(ptr, writer.AsNativeArray().GetUnsafeReadOnlyPtr(), writer.Length);
        }
    }
}