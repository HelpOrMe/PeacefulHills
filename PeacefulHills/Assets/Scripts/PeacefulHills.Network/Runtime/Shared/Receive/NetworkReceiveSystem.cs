using PeacefulHills.Extensions;
using PeacefulHills.Network.Packet;
using PeacefulHills.Network.Profiling;
using Unity.Entities;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class NetworkReceiveSystem : SystemBase
    {
        private EntityQuery _connectionsQuery;

        protected override void OnCreate()
        {
            _connectionsQuery = GetEntityQuery(
                ComponentType.ReadOnly<DriverConnection>(),
                ComponentType.ReadOnly<PacketAgentsPool>());
        }

        protected override void OnUpdate()
        {
            var driver = World.GetExtension<INetworkDriverInfo>();

            var receiveJob = new NetworkReceiveJob
            {
                PacketAgentPoolHandle = GetBufferTypeHandle<PacketAgentsPool>(true),
                ConnectionLinksHandle = GetComponentTypeHandle<DriverConnection>(true),
                ReceiveBufferFromEntity = GetBufferFromEntity<PacketReceiveBuffer>(),
                Driver = driver.Concurrent,
                BytesReceivedCounter = NetworkProfilerCounters.BytesReceived
            };

            driver.Dependency = receiveJob.Schedule(_connectionsQuery, driver.Dependency);
            Dependency = driver.Dependency;
        }
    }
}