using PeacefulHills.ECS;
using PeacefulHills.Network.Messages;
using Unity.Entities;

[assembly:RegisterGenericComponentType(typeof(ExtensionSingleton<IMessagesRegistry>))]

namespace PeacefulHills.Network.Messages
{
    public interface IMessagesRegistry : IWorldExtension
    {
        public uint Register<TMessage>() where TMessage : IMessage;

        public MessageInfo GetInfoById(uint id);

        public uint GetIdByStableHash(ulong stableHash);
    }
}