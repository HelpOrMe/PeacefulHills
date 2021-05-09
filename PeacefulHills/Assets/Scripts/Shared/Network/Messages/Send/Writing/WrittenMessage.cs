using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public struct WrittenMessage : IComponentData
    {
        public int Index;
        public unsafe byte* Bytes;
    }
}