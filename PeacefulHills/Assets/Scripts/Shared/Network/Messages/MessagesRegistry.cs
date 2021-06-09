using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public class MessagesRegistry : IMessagesRegistry
    {
        private uint _lastMessageId;
        private NativeHashMap<ulong, uint> _messageIdsByStableHash;
        private NativeHashMap<uint, MessageInfo> _messagesById;

        public MessagesRegistry()
        {
            _messageIdsByStableHash = new NativeHashMap<ulong, uint>(1, Allocator.Persistent);
            _messagesById = new NativeHashMap<uint, MessageInfo>(1, Allocator.Persistent);
        }

        public uint Register<TMessage>() where TMessage : IMessage
        {
            uint id = _lastMessageId++;
            TypeManager.TypeInfo typeInfo = TypeManager.GetTypeInfo<TMessage>();
            _messagesById[_lastMessageId] = new MessageInfo(typeInfo, _lastMessageId);
            _messageIdsByStableHash[typeInfo.StableTypeHash] = id;
            return id;
        }

        public MessageInfo GetInfoById(uint id)
        {
            return _messagesById[id];
        }

        public uint GetIdByStableHash(ulong stableHash)
        {
            return _messageIdsByStableHash[stableHash];
        }
    }
}