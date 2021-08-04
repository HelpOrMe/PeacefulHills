using PeacefulHills.Extensions.World;
using Unity.Entities;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class NetworkDriverSystem : SystemBase
    {
        protected override void OnCreate()
        {
            this.RequireExtension<INetwork>();
        }

        protected override void OnUpdate()
        {
            var network = World.GetExtension<INetwork>();
            network.DriverDependency.Complete();
            network.DriverDependency = network.Driver.ScheduleUpdate();
        }
    }
}