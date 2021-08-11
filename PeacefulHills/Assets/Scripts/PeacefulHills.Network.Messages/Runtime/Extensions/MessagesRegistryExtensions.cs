using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public static class MessagesRegistryExtensions
    {
        public static ushort GetId<TMessage>(this IMessageRegistry registry)
            where TMessage : IMessage
        {
            return registry.GetIdByStableHash(TypeManager.GetTypeInfo<TMessage>().StableTypeHash);
        }

        public static MessageInfo GetInfo<TMessage>(this IMessageRegistry registry)
            where TMessage : IMessage
        {
            return registry.GetInfoByStableHash(TypeManager.GetTypeInfo<TMessage>().StableTypeHash);
        }

        public static MessageInfo GetInfoByStableHash(this IMessageRegistry registry, ulong stableHash)
        {
            return registry.GetInfoById(registry.GetIdByStableHash(stableHash));
        }
    }
}