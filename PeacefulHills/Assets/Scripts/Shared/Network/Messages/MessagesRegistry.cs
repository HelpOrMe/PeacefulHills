using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public class MessagesRegistry : IMessagesRegistry
    {
        private ushort _lastMessageId;
        private NativeHashMap<ulong, ushort> _messageIdsByStableHash;
        private NativeHashMap<ushort, MessageInfo> _messagesById;

        public MessagesRegistry()
        {
            _messageIdsByStableHash = new NativeHashMap<ulong, ushort>(1, Allocator.Persistent);
            _messagesById = new NativeHashMap<ushort, MessageInfo>(1, Allocator.Persistent);
        }

        public uint Register<TMessage>() where TMessage : IMessage
        {
            ushort id = _lastMessageId++;
            TypeManager.TypeInfo typeInfo = TypeManager.GetTypeInfo<TMessage>();
            _messagesById[_lastMessageId] = new MessageInfo(typeInfo, _lastMessageId);
            _messageIdsByStableHash[typeInfo.StableTypeHash] = id;
            return id;
        }

        public MessageInfo GetInfoById(ushort id)
        {
            return _messagesById[id];
        }

        public ushort GetIdByStableHash(ulong stableHash)
        {
            return _messageIdsByStableHash[stableHash];
        }

        public void Dispose()
        {
            _messageIdsByStableHash.Dispose();
            _messagesById.Dispose();
        }
    }
}