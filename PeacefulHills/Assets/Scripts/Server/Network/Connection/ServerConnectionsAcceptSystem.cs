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
            RequireSingletonForUpdate<NetworkSingleton>();
        }

        protected override void OnUpdate()
        {
            INetwork network = this.GetNetworkFromSingleton();
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