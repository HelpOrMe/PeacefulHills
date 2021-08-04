using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public static class MessagesRegistryExtensions
    {
        public static ushort GetId<TMessage>(this IMessagesRegistry registry)
            where TMessage : IMessage
        {
            return registry.GetIdByStableHash(TypeManager.GetTypeInfo<TMessage>().StableTypeHash);
        }

        public static MessageInfo GetInfo<TMessage>(this IMessagesRegistry registry)
            where TMessage : IMessage
        {
            return registry.GetInfoByStableHash(TypeManager.GetTypeInfo<TMessage>().StableTypeHash);
        }

        public static MessageInfo GetInfoByStableHash(this IMessagesRegistry registry, ulong stableHash)
        {
            return registry.GetInfoById(registry.GetIdByStableHash(stableHash));
        }
    }
}