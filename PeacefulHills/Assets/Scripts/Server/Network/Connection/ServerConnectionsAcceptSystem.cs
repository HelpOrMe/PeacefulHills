using PeacefulHills.ECS.World;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    [UpdateInGroup(typeof(ConnectionSimulationGroup))]
    public class ServerConnectionsAcceptSystem : SystemBase
    {
        private EndConnectionSimulationBuffer _buffer;

        protected override void OnCreate()
        {
            _buffer = World.GetOrCreateSystem<EndConnectionSimulationBuffer>();
        }

        protected override void OnUpdate()
        {
            var network = World.GetExtension<INetwork>();
            EntityCommandBuffer commandBuffer = _buffer.CreateCommandBuffer();

            var connectionsAcceptJob = new ServerConnectionsAcceptJob
            {
                Driver = network.Driver, 
                CommandBuffer = commandBuffer
            };

            network.DriverDependency = connectionsAcceptJob.Schedule(network.DriverDependency);
            Dependency = network.DriverDependency;
            
            _buffer.AddJobHandleForProducer(network.DriverDependency);
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