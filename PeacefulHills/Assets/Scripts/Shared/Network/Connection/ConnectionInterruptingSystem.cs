using PeacefulHills.ECS.World;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    [UpdateInGroup(typeof(ConnectionSimulationGroup))]
    public class ConnectionInterruptingSystem : SystemBase
    {
        private EndConnectionSimulationBuffer _buffer;

        protected override void OnCreate()
        {
            _buffer = World.GetOrCreateSystem<EndConnectionSimulationBuffer>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer commandBuffer = _buffer.CreateCommandBuffer();
            
            var network = World.GetExtension<INetwork>();
            NetworkDriver driver = network.Driver;
            
            network.DriverDependency = Entities
               .WithName("Interrupt_connections")
               .WithAll<ConnectionInterrupt>()
               .ForEach((Entity entity, ref ConnectionWrapper connectionWrapper) =>
                {
                    if (!connectionWrapper.Connection.IsCreated)
                    {
                        driver.Disconnect(connectionWrapper.Connection);
                    }
                    commandBuffer.DestroyEntity(entity);
                })
               .Schedule(network.DriverDependency);

            Dependency = network.DriverDependency;
            _buffer.AddJobHandleForProducer(network.DriverDependency);
        }
    }
}