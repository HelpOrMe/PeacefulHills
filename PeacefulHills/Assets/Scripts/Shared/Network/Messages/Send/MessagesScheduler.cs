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

        public unsafe void Schedule(DynamicBuffer<MessagesSendBuffer> buffer, TMessage message)
        {
            var serializer = default(TMessageSerializer);
            
            const int messageIdSize = 2;
            var writer = new DataStreamWriter(sizeof(TMessage) + messageIdSize, Allocator.Temp);
                
            writer.WriteUShort(MessageId);
            serializer.Write(in message, ref writer);

            int previousBufferLength = buffer.Length;
            buffer.ResizeUninitialized(buffer.Length + writer.Length);
            byte* ptr = (byte*) buffer.GetUnsafePtr() + previousBufferLength;
             
            UnsafeUtility.MemCpy(ptr, writer.AsNativeArray().GetUnsafeReadOnlyPtr(), writer.Length);
        }
    }
}