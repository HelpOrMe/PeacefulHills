using PeacefulHills.ECS;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class ServerConnectionsInterruptingSystem : SystemBase
    {
        private EndNetworkSimulationBuffer _endSimulation;
        
        protected override void OnCreate()
        {
            _endSimulation = World.GetOrCreateSystem<EndNetworkSimulationBuffer>();
           this.RequireExtension<INetwork>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer commandBuffer = _endSimulation.CreateCommandBuffer();
            var network = World.GetExtension<INetwork>();
            
            network.LastDriverJobHandle = Entities
                .WithName("Clear_interrupted_connections")
                .WithEntityQueryOptions(EntityQueryOptions.FilterWriteGroup)
                .WithAll<InterruptedNetworkConnection>()
                .ForEach((Entity entity, in NetworkConnectionWrapper connectionTarget) =>
                {
                    commandBuffer.DestroyEntity(entity);
                })
                .Schedule(network.LastDriverJobHandle);
            
            NetworkDriver driver = network.Driver;
            
            network.LastDriverJobHandle = Entities
                .WithName("Interrupt_connections")
                .WithEntityQueryOptions(EntityQueryOptions.FilterWriteGroup)
                .WithAll<InterruptNetworkConnection>()
                .ForEach((Entity entity, ref NetworkConnectionWrapper connectionTarget) =>
                {
                    if (!connectionTarget.Connection.IsCreated)
                    {
                        connectionTarget.Connection.Disconnect(driver);
                    }
                })
                .Schedule(network.LastDriverJobHandle);
        }
    }
}