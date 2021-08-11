using PeacefulHills.Extensions;
using Unity.Entities;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class NetworkDriverSystem : SystemBase
    {
        protected override void OnCreate()
        {
            this.RequireExtension<INetworkDriverInfo>();
        }

        protected override void OnUpdate()
        {
            var driver = World.GetExtension<INetworkDriverInfo>();
            driver.Dependency.Complete();
            driver.Dependency = driver.Current.ScheduleUpdate();
        }
    }
}