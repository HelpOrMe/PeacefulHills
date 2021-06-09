using PeacefulHills.ECS.World;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class ServerConnectionsAcceptSystem : SystemBase
    {
        private BeginNetworkSimulationBuffer _endSimulationBuffer;

        protected override void OnCreate()
        {
            _endSimulationBuffer = World.GetOrCreateSystem<BeginNetworkSimulationBuffer>();
        }

        protected override void OnUpdate()
        {
            var network = World.GetExtension<INetwork>();
            EntityCommandBuffer commandBuffer = _endSimulationBuffer.CreateCommandBuffer();

            var connectionsAcceptJob = new ServerConnectionsAcceptJob
            {
                Driver = network.Driver, CommandBuffer = commandBuffer
            };

            network.DriverDependency = connectionsAcceptJob.Schedule(network.DriverDependency);
            Dependency = network.DriverDependency;
            _endSimulationBuffer.AddJobHandleForProducer(network.DriverDependency);
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
                    if (connection.PopEvent(Driver, out DataStreamReader _) != NetworkEvent.Type.Empty)
                    {
                        continue;
                    }

                    Entity entity = CommandBuffer.CreateEntity();
                    CommandBuffer.SetComponent(entity, new NetworkConnectionWrapper {Connection = connection});
                }
            }
        }
    }
}