using Unity.Entities;
using Unity.Networking.Transport;
using UnityEngine;

namespace PeacefulHills.Network.Connection
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class ServerConnectionsInterruptingSystem : SystemBase
    {
        private EndServerSimulationCommandBufferSystem _endSimulation;
        
        protected override void OnCreate()
        {
            _endSimulation = World.GetOrCreateSystem<EndServerSimulationCommandBufferSystem>();
            RequireSingletonForUpdate<NetworkSingleton>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer commandBuffer = _endSimulation.CreateCommandBuffer();
            Network network = this.GetNetworkFromSingleton();
            
            network.LastDriverJobHandle = Entities
                .WithName("Clear_interrupted_connections")
                .WithAll<InterruptedConnection>()
                .ForEach((Entity entity, in NetworkStreamConnection streamConnection) =>
                {
                    commandBuffer.DestroyEntity(entity);
                })
                .Schedule(network.LastDriverJobHandle);
            
            NetworkDriver driver = network.Driver;
            
            network.LastDriverJobHandle = Entities
                .WithName("Interrupt_connections")
                .WithAll<InterruptConnection>()
                .ForEach((Entity entity, ref NetworkStreamConnection streamConnection) =>
                {
                    if (!streamConnection.Connection.IsCreated)
                    {
                        streamConnection.Connection.Disconnect(driver);
                    }
                })
                .Schedule(network.LastDriverJobHandle);
        }
    }
}