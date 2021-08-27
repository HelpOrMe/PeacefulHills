using PeacefulHills.Extensions;
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

            var driver = World.GetExtension<INetworkDriverInfo>();
            NetworkDriver currentDriver = driver.Current;

            driver.Dependency = Entities
               .WithName("Interrupt_connections")
               .WithAll<ConnectionInterrupt>()
               .ForEach((Entity entity, ref ConnectionWrapper connection) =>
                {
                    if (!connection.Value.IsCreated)
                    {
                        currentDriver.Disconnect(connection.Value);
                    }
                    commandBuffer.DestroyEntity(entity);
                })
               .Schedule(driver.Dependency);

            Dependency = driver.Dependency;
            _buffer.AddJobHandleForProducer(driver.Dependency);
        }
    }
}