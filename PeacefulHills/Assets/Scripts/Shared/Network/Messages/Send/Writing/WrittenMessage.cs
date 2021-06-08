using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public struct WrittenMessage : IComponentData
    {
        public int Index;
        public int Size;
        public unsafe byte* Bytes;
    }
}