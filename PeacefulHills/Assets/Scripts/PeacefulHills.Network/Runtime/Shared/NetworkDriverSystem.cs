using PeacefulHills.Extensions;
using Unity.Entities;
using Unity.Mathematics;

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
            math.mul()
            var driver = World.GetExtension<INetworkDriverInfo>();
            driver.Dependency.Complete();
            driver.Dependency = driver.Current.ScheduleUpdate();
        }
    }
}