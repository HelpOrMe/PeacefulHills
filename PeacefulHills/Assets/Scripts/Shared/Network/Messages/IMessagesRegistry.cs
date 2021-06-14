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
        public NativeList<MessageInfo> Messages { get; }
        
        public ushort Register<TMessage>() where TMessage : IMessage;

        public MessageInfo GetInfoById(ushort id);

        public ushort GetIdByStableHash(ulong stableHash);
    }
}