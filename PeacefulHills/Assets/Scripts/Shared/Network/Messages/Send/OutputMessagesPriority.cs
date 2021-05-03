using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public struct OutputMessagesPriority : IComponentData
    {
        public int Priority;
    }
}