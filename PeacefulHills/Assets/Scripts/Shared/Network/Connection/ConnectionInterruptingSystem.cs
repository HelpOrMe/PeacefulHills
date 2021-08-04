using PeacefulHills.ECS.World;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(ConnectionSimulationGroup))]
    public class ConnectionInterruptingSystem : SystemBase
    {
        private EndNetworkSimulationBuffer _buffer;

        protected override void OnCreate()
        {
            _buffer = World.GetOrCreateSystem<EndNetworkSimulationBuffer>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer commandBuffer = _buffer.CreateCommandBuffer();

            var network = World.GetExtension<INetwork>();
            NetworkDriver driver = network.Driver;

            network.DriverDependency = Entities
               .WithName("Interrupt_connections")
               .WithAll<ConnectionInterrupt>()
               .ForEach((Entity entity, ref ConnectionWrapper connection) =>
                {
                    if (!connection.Value.IsCreated)
                    {
                        driver.Disconnect(connection.Value);
                    }
                    commandBuffer.DestroyEntity(entity);
                })
               .Schedule(network.DriverDependency);

            Dependency = network.DriverDependency;
            _buffer.AddJobHandleForProducer(network.DriverDependency);
        }
    }
}