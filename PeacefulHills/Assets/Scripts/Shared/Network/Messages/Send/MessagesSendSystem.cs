using PeacefulHills.ECS.World;
using Unity.Entities;
using Unity.Profiling;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(MessagesSimulationGroup))]
    [UpdateAfter(typeof(MessagesWriteGroup))]
    public class MessagesSendSystem : SystemBase
    {
        private EntityQuery _connectionsQuery;
        
        protected override void OnCreate()
        {
            _connectionsQuery = GetEntityQuery(ComponentType.ReadOnly<ConnectionWrapper>(), typeof(MessagesSendBuffer));
        }

        protected override void OnUpdate()
        {
            var counter = new ProfilerCounterValue<int>(ProfilerCategory.Network, "Network.Messages.Send", ProfilerMarkerDataUnit.Bytes);
            counter.Value += 1;
            
            var registry = World.GetExtension<IMessagesRegistry>();
            var network = World.GetExtension<INetwork>();
            
            var job = new MessagesSendJob
            {
                ConnectionHandle = GetComponentTypeHandle<ConnectionWrapper>(true),
                Messages = registry.Messages,
                MessagesBufferHandle = GetBufferTypeHandle<MessagesSendBuffer>(),
                Pipeline = network.ReliablePipeline,
                Driver = network.DriverConcurrent
            };

            network.DriverDependency = job.ScheduleParallel(_connectionsQuery, network.DriverDependency);
            Dependency = network.DriverDependency;
        }
    }
}