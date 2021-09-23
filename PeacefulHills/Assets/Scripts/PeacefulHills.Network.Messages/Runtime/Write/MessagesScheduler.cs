using PeacefulHills.Network.Packet;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public readonly struct MessagesScheduler<TMessage, TMessageSerializer>
        where TMessage : unmanaged, IMessage
        where TMessageSerializer : unmanaged, IMessageSerializer<TMessage>
    {
        private readonly ushort _messageId;

        public MessagesScheduler(ushort messageId)
        {
            _messageId = messageId;
        }

        public unsafe void Schedule(DynamicBuffer<PacketSendBuffer> sendBuffer, TMessage message)
        {
            var serializer = default(TMessageSerializer);

            const int messageIdSize = 2;
            var writer = new DataStreamWriter(sizeof(TMessage) + messageIdSize, Allocator.Temp);

            writer.WriteUShort(_messageId);
            serializer.Serialize(ref writer, in message);

            int previousBufferLength = sendBuffer.Length;
            sendBuffer.ResizeUninitialized(sendBuffer.Length + writer.Length);
            byte* ptr = (byte*) sendBuffer.GetUnsafePtr() + previousBufferLength;

            UnsafeUtility.MemCpy(ptr, writer.AsNativeArray().GetUnsafeReadOnlyPtr(), writer.Length);
        }
    }
}