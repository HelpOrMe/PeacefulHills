using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public static class MessagesRegistryExtensions
    {
        public static MessageInfo GetInfo<TMessage>(this IMessagesRegistry registry) 
            where TMessage : IMessage 
            => registry.GetInfoByStableHash(TypeManager.GetTypeInfo<TMessage>().StableTypeHash);

        public static MessageInfo GetInfoByStableHash(this IMessagesRegistry registry, ulong stableHash)
            => registry.GetInfoById(registry.GetIdByStableHash(stableHash));
    }
}