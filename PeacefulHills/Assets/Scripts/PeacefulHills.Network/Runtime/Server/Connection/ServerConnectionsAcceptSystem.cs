using PeacefulHills.Extensions;
using Unity.Entities;
using Unity.Jobs;

namespace PeacefulHills.Network.Connection
{
    [UpdateInGroup(typeof(ConnectionSimulationGroup))]
    public class ServerConnectionsAcceptSystem : SystemBase
    {
        private EndNetworkSimulationBuffer _buffer;

        protected override void OnCreate()
        {
            _buffer = World.GetOrCreateSystem<EndNetworkSimulationBuffer>();
        }

        protected override void OnUpdate()
        {
            var driver = World.GetExtension<INetworkDriverInfo>();
            EntityCommandBuffer commandBuffer = _buffer.CreateCommandBuffer();

            var connectionsAcceptJob = new ServerConnectionsAcceptJob
            {
                Driver = driver.Current, 
                CommandBuffer = commandBuffer,
                ConnectionBuilder = World.GetExtension<ConnectionBuilder>()
            };

            driver.Dependency = connectionsAcceptJob.Schedule(driver.Dependency);
            Dependency = driver.Dependency;
            
            _buffer.AddJobHandleForProducer(driver.Dependency);
        }
    }
}