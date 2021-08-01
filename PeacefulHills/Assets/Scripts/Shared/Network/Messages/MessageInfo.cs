using Unity.Burst;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public readonly struct MessageInfo
    {
        public readonly TypeManager.TypeInfo TypeInfo;
        public readonly ushort Id;
        public readonly FunctionPointer<DeserializeAction> Deserialize;

        public MessageInfo(TypeManager.TypeInfo typeInfo, ushort id, FunctionPointer<DeserializeAction> deserialize)
        {
            TypeInfo = typeInfo;
            Id = id;
            Deserialize = deserialize;
        }
    }
}