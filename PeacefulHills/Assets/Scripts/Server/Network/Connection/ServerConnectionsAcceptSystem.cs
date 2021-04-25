using Unity.Entities;
using Unity.Jobs;

namespace PeacefulHills.Network.Connection
{
    [UpdateInGroup(typeof(ServerNetworkUpdateGroup))]
    public class ServerConnectionsAcceptSystem : SystemBase
    {
        private BeginServerSimulationCommandBufferSystem _endSimulation;
        
        protected override void OnCreate()
        {
            _endSimulation = World.GetOrCreateSystem<BeginServerSimulationCommandBufferSystem>();
            RequireSingletonForUpdate<NetworkSingleton>();
        }

        protected override void OnUpdate()
        {
            Network network = NetworkManager.GetNetwork(GetSingleton<NetworkSingleton>().Handle);
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