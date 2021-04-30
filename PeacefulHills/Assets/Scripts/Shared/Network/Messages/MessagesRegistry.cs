using System;
using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public struct MessagesRegistry
    {
        private NativeHashMap<ulong, uint> _messageIds;
        private uint _lastMessageId;

        public uint GetOrRegisterId<TMessage>() where TMessage : IMessage
        {
            ulong stableHash = TypeManager.GetTypeInfo<TMessage>().StableTypeHash;
            if (_messageIds.ContainsKey(stableHash))
            {
                return _messageIds[stableHash];
            }
            return _messageIds[stableHash] = _lastMessageId++;
        }

        public uint RegisterId<TMessage>() where TMessage : IMessage
        {
            ulong stableHash = TypeManager.GetTypeInfo<TMessage>().StableTypeHash;

            if (_messageIds.ContainsKey(stableHash))
            {
                throw new ArgumentException($"{nameof(TMessage)} id already registered.");
            }
            return _messageIds[stableHash] = _lastMessageId++;
        }

        public uint GetId<TMessage>() where TMessage : IMessage
        {
            ulong stableHash = TypeManager.GetTypeInfo<TMessage>().StableTypeHash;
            
            if (!_messageIds.ContainsKey(stableHash))
            {
                throw new ArgumentException($"{nameof(TMessage)} is not registered.");
            }
            
            return _messageIds[stableHash];
        }
    }
}