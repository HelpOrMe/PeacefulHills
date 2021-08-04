using System;
using PeacefulHills.ECS.World;
using PeacefulHills.Network.Messages;
using Unity.Collections;
using Unity.Entities;

[assembly: RegisterGenericComponentType(typeof(ExtensionSingleton<IMessagesRegistry>))]

namespace PeacefulHills.Network.Messages
{
    public interface IMessagesRegistry : IWorldExtension, IDisposable
    {
        NativeList<MessageInfo> Messages { get; }

        void Register<TMessage, TMessageSerializer>()
            where TMessage : unmanaged, IMessage
            where TMessageSerializer : unmanaged, IMessageSerializer<TMessage>;

        MessageInfo GetInfoById(ushort id);

        ushort GetIdByStableHash(ulong stableHash);
    }
}