using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public readonly struct MessageInfo
    {
        public readonly TypeManager.TypeInfo TypeInfo;
        public readonly uint Id;

        public MessageInfo(TypeManager.TypeInfo typeInfo, uint id)
        {
            TypeInfo = typeInfo;
            Id = id;
        }
    }
}