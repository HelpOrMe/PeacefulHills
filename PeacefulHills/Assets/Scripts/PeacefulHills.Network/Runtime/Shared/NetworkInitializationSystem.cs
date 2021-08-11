using PeacefulHills.Extensions;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    [UpdateAfter(typeof(BeginNetworkInitializationBuffer))]
    public class NetworkInitializationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            var driver = NetworkDriver.Create();
            var driverInfo = new NetworkDriverInfo
            {
                Current = driver,
                Concurrent = driver.ToConcurrent(),
                ReliablePipeline = driver.CreatePipeline(typeof(ReliableSequencedPipelineStage)),
                UnreliablePipeline = driver.CreatePipeline(typeof(NullPipelineStage))
            };

            World.SetExtension<INetworkDriverInfo>(driverInfo);
        }

        protected override void OnDestroy()
        {
            var driver = World.GetExtension<INetworkDriverInfo>();
            driver.Dispose();
        }

        protected override void OnUpdate()
        {
        }
    }
}