using PeacefulHills.ECS.World;
using PeacefulHills.Network.Profiling;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Receive
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class NetworkReceiveSystem : SystemBase
    {
        private EntityQuery _connectionsQuery;
        
        protected override void OnCreate()
        {
            _connectionsQuery = GetEntityQuery(
                ComponentType.ReadOnly<ConnectionWrapper>(),
                ComponentType.ReadOnly<NetworkReceiveBufferPool>());
        }

        protected override void OnUpdate()
        {
            var network = World.GetExtension<INetwork>();
            NetworkDriver.Concurrent driver = network.DriverConcurrent;

            var receiveJob = new NetworkReceiveJob
            {
                ReceiveBufferPoolHandle = GetBufferTypeHandle<NetworkReceiveBufferPool>(true),
                ConnectionsHandle = GetComponentTypeHandle<ConnectionWrapper>(true),
                ReceiveBufferFromEntity = GetBufferFromEntity<NetworkReceiveBuffer>(),
                Driver = driver,
                BytesReceivedCounter = NetworkProfilerCounters.BytesReceived
            };

            network.DriverDependency = receiveJob.Schedule(_connectionsQuery, network.DriverDependency);
            Dependency = network.DriverDependency;
        }
    }
}