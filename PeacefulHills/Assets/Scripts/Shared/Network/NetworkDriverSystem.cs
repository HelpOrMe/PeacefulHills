using PeacefulHills.ECS.World;
using Unity.Entities;

namespace PeacefulHills.Network
{
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
            // network.DriverConcurrent = network.Driver.ToConcurrent();
            network.Driver.ScheduleUpdate();
        }
    }
}