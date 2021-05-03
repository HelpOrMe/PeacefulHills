namespace PeacefulHills.Network.Messages
{
    public interface IMessagesRegistry
    {
        public uint GetOrRegisterId<TMessage>() where TMessage : IMessage;

        public uint RegisterId<TMessage>() where TMessage : IMessage;

        public uint GetId<TMessage>() where TMessage : IMessage;
    }
}