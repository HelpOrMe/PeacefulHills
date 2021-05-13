using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public class MessagesRegistry : IMessagesRegistry
    {
        private NativeHashMap<uint, MessageInfo> _messagesById;
        private NativeHashMap<ulong, uint> _messageIdsByStableHash;
        
        private uint _lastMessageId;

        public uint Register<TMessage>() where TMessage : IMessage
        {
            uint id = _lastMessageId++;
            TypeManager.TypeInfo typeInfo = TypeManager.GetTypeInfo<TMessage>();
            _messagesById[_lastMessageId] = new MessageInfo(typeInfo, _lastMessageId++);
            return id;
        }

        public MessageInfo GetInfoById(uint id) 
            => _messagesById[id];

        public uint GetIdByStableHash(ulong stableHash) 
            => _messageIdsByStableHash[stableHash];
    }
}