using System;
using PeacefulHills.ECS.World;
using PeacefulHills.Network.Messages;
using Unity.Entities;

[assembly: RegisterGenericComponentType(typeof(ExtensionSingleton<IMessagesRegistry>))]

namespace PeacefulHills.Network.Messages
{
    public interface IMessagesRegistry : IWorldExtension, IDisposable
    {
        public uint Register<TMessage>() where TMessage : IMessage;

        public MessageInfo GetInfoById(ushort id);

        public ushort GetIdByStableHash(ulong stableHash);
    }
}