using Unity.Entities;
using Unity.Jobs;

namespace PeacefulHills.Network.Messages
{
    public struct MessageWriteDependencies : IComponentData
    {
        public JobHandle Handle;
    }
}