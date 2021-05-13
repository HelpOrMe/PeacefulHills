using PeacefulHills.ECS;
using Unity.Entities;
using Unity.Jobs;

namespace PeacefulHills.Network.Connection
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class ServerConnectionsAcceptSystem : SystemBase
    {
        private BeginNetworkSimulationBuffer _endSimulation;
        
        protected override void OnCreate()
        {
            _endSimulation = World.GetOrCreateSystem<BeginNetworkSimulationBuffer>();
            this.RequireExtension<INetwork>();
        }

        protected override void OnUpdate()
        {
            var network = World.GetExtension<INetwork>();
            EntityCommandBuffer commandBuffer = _endSimulation.CreateCommandBuffer();

            var connectionsAcceptJob = new ServerConnectionsAcceptJob
            {
                Driver = network.Driver,
                CommandBuffer = commandBuffer
            };

            network.LastDriverJobHandle = connectionsAcceptJob.Schedule(network.LastDriverJobHandle);
        }
    }
}