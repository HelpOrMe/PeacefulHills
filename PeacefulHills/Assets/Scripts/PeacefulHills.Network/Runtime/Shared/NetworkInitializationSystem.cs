using PeacefulHills.Extensions;
using Unity.Entities;
using Unity.Networking.Transport;
using UnityEngine;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    public class NetworkInitializationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            Debug.Log("Network init");
            var driver = NetworkDriver.Create();
            var driverInfo = new NetworkDriverInfo
            {
                Current = driver,
                Concurrent = driver.ToConcurrent(),
                ReliablePipeline = driver.CreatePipeline(typeof(ReliableSequencedPipelineStage)),
                UnreliablePipeline = driver.CreatePipeline(typeof(NullPipelineStage))
            };

            World.SetExtension<INetworkDriverInfo>(driverInfo);
            // todo: *Here must be a sorted packet group init*
            // World.SetExtension(new ConnectionBuilder(Allocator.Persistent));
        }

        protected override void OnDestroy()
        {
            World.GetExtension<INetworkDriverInfo>().Dispose();
        }

        protected override void OnUpdate()
        {
        }
    }
}