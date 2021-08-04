using Unity.Burst;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public readonly struct MessageInfo
    {
        public readonly ushort Id;
        public readonly TypeManager.TypeInfo TypeInfo;
        public readonly FunctionPointer<DeserializeAction> Deserialize;

        public MessageInfo(ushort id, TypeManager.TypeInfo typeInfo, FunctionPointer<DeserializeAction> deserialize)
        {
            Id = id;
            TypeInfo = typeInfo;
            Deserialize = deserialize;
        }
    }
}