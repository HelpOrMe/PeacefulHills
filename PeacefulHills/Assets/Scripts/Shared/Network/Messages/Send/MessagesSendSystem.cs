using PeacefulHills.ECS.World;
using PeacefulHills.Network.Connection;
using Unity.Entities;

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
            _connectionsQuery.AddChangedVersionFilter(typeof(MessagesSendBuffer));
        }

        protected override void OnUpdate()
        {
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

            Dependency = job.ScheduleParallel(_connectionsQuery, network.DriverDependency);
            network.DriverDependency = Dependency;
        }
    }
}