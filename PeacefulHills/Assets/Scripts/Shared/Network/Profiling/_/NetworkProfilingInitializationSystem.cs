using PeacefulHills.ECS.World;
using Unity.Entities;

namespace PeacefulHills.Network.Profiling
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    public class NetworkProfilingInitializationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            World.SetExtension<INetworkStatsCollector>(new NetworkStatsCollector());
        }

        protected override void OnDestroy()
        {
            World.GetExtension<INetworkStatsCollector>().Dispose();
        }

        protected override void OnUpdate()
        {
            
        }
    }
}