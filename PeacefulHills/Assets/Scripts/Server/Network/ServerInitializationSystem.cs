using PeacefulHills.Network.Exceptions;
using Unity.Entities;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Utilities;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    public class ServerInitializationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            InitializeNetworkSingleton();
        }
        
        private void InitializeNetworkSingleton()
        {
            NetworkHandle networkHandle = NetworkManager.AddNetwork(InitializeNetwork());
            Entity entity = EntityManager.CreateEntity(typeof(NetworkSingleton));
            EntityManager.SetComponentData(entity, new NetworkSingleton {Handle = networkHandle});
        }

        private Network InitializeNetwork()
        {
            var network = new ServerNetwork();
         
            var reliabilityParams = new ReliableUtility.Parameters { WindowSize = 32 };
            var fragmentationParams = new FragmentationUtility.Parameters { PayloadCapacity = 16 * 1024 };
            
            network.Driver = NetworkDriver.Create(reliabilityParams, fragmentationParams);
            network.Pipelines = new ServerNetwork.NetworkPipelines
            {
                Unreliable = network.Driver.CreatePipeline(typeof(NullPipelineStage)),
                Reliable = network.Driver.CreatePipeline(typeof(ReliableSequencedPipelineStage)),
                UnreliableFragmented = network.Driver.CreatePipeline(typeof(FragmentationPipelineStage))
            };

            NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
            endpoint.Port = 9000;
            
            if (network.Driver.Bind(endpoint) != 0)
            {
                throw new NetworkSimulationException("Unable to bind port " + endpoint.Port);
            }

            network.Driver.Listen();
            return network;
        }

        protected override void OnUpdate() { }
    }
}
