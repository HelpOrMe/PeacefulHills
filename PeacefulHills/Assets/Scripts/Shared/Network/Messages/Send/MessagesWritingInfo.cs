using Unity.Entities;
using Unity.Jobs;

namespace PeacefulHills.Network.Messages
{
    public struct MessagesWritingInfo : IComponentData
    {
        public JobHandle Handle;
    }
}