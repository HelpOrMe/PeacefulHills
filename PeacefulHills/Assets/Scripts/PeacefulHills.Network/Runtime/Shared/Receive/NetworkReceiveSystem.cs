using PeacefulHills.Extensions;
using PeacefulHills.Network.Profiling;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
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
            var driver = World.GetExtension<INetworkDriverInfo>();

            var receiveJob = new NetworkReceiveJob
            {
                ReceiveBufferPoolHandle = GetBufferTypeHandle<NetworkReceiveBufferPool>(true),
                ConnectionsHandle = GetComponentTypeHandle<ConnectionWrapper>(true),
                ReceiveBufferFromEntity = GetBufferFromEntity<NetworkReceiveBuffer>(),
                Driver = driver.Concurrent,
                BytesReceivedCounter = NetworkProfilerCounters.BytesReceived
            };

            driver.Dependency = receiveJob.Schedule(_connectionsQuery, driver.Dependency);
            Dependency = driver.Dependency;
        }
    }
}