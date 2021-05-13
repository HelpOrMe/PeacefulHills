using PeacefulHills.ECS;

namespace PeacefulHills.Network.Messages
{
    public interface IMessagesRegistry : IWorldExtension
    {
        public uint Register<TMessage>() where TMessage : IMessage;

        public MessageInfo GetInfoById(uint id);

        public uint GetIdByStableHash(ulong stableHash);
    }
}