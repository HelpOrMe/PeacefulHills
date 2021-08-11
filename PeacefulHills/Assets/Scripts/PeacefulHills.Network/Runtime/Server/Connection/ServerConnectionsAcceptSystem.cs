using PeacefulHills.Extensions;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Networking.Transport;

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
                CommandBuffer = commandBuffer
            };

            driver.Dependency = connectionsAcceptJob.Schedule(driver.Dependency);
            Dependency = driver.Dependency;
            
            _buffer.AddJobHandleForProducer(driver.Dependency);
        }

        [BurstCompile]
        public struct ServerConnectionsAcceptJob : IJob
        {
            public NetworkDriver Driver;
            public EntityCommandBuffer CommandBuffer;

            public void Execute()
            {
                NetworkConnection connection;
                while ((connection = Driver.Accept()) != default)
                {
                    if (connection.PopEvent(Driver, out DataStreamReader _) == NetworkEvent.Type.Empty)
                    {
                        ConnectionBuilder.CreateConnection(CommandBuffer, connection);
                    }
                }
            }
        }
    }
}