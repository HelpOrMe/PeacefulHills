using Unity.Entities;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Utilities;

namespace PeacefulHills.Network
{
    public class ClientInitializationSystem : SystemBase
    {
        public NetworkDriver Driver { get; private set; }
        public NetworkPipelines Pipelines { get; private set; }
        
        public struct NetworkPipelines
        {
            public NetworkPipeline UnreliablePipeline;
            public NetworkPipeline ReliablePipeline;
        }
        
        protected override void OnCreate()
        {
            InitializeDriver();
        }
        
        protected void InitializeDriver()
        {
            var reliabilityParams = new ReliableUtility.Parameters { WindowSize = 32 };
            var fragmentationParams = new FragmentationUtility.Parameters { PayloadCapacity = 16 * 1024 };
            
            Driver = NetworkDriver.Create(reliabilityParams, fragmentationParams);

            Pipelines = new NetworkPipelines
            {
                UnreliablePipeline = Driver.CreatePipeline(typeof(NullPipelineStage)),
                ReliablePipeline = Driver.CreatePipeline(typeof(ReliableSequencedPipelineStage)),
            };
        }
        
        protected override void OnUpdate() { }
    }
}