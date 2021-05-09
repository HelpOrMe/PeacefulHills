using Unity.Entities;
using Unity.Jobs;

namespace PeacefulHills.Network.Messages
{
    public struct MessagesSendingDependency : IComponentData
    {
        public JobHandle Handle;
    }
}