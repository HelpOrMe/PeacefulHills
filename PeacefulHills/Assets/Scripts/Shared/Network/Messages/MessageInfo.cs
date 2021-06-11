using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public readonly struct MessageInfo
    {
        public readonly TypeManager.TypeInfo TypeInfo;
        public readonly ushort Id;

        public MessageInfo(TypeManager.TypeInfo typeInfo, ushort id)
        {
            TypeInfo = typeInfo;
            Id = id;
        }
    }
}