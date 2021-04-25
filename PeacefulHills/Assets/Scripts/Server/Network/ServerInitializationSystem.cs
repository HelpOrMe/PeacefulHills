using Unity.Entities;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Utilities;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(ServerInitializationGroup))]
    public class ServerInitializationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            InitializeNetwork();
        }
        
        protected void InitializeNetwork()
        {
            var reliabilityParams = new ReliableUtility.Parameters { WindowSize = 32 };
            var fragmentationParams = new FragmentationUtility.Parameters { PayloadCapacity = 16 * 1024 };

            var network = new Network();
            
            network.Driver = NetworkDriver.Create(reliabilityParams, fragmentationParams);
            network.Pipelines = new Network.NetworkPipelines
            {
                Unreliable = network.Driver.CreatePipeline(typeof(NullPipelineStage)),
                Reliable = network.Driver.CreatePipeline(typeof(ReliableSequencedPipelineStage)),
                UnreliableFragmented = network.Driver.CreatePipeline(typeof(FragmentationPipelineStage))
            };
            
            NetworkHandle networkHandle = NetworkManager.AddNetwork(network);
            
            Entity entity = EntityManager.CreateEntity(typeof(NetworkSingleton));
            EntityManager.SetComponentData(entity, new NetworkSingleton {Handle = networkHandle});
        }

        protected override void OnUpdate() { }
    }
}
