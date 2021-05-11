using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace PeacefulHills.Network.Messages
{
    public class PackingMessagesSystem : SystemBase
    {
        private EntityQuery _messagesQuery;
        private EndNetworkSimulationBuffer _endSimulationBuffer;
        
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<MessagesSendingDependency>();

            _endSimulationBuffer = World.GetOrCreateSystem<EndNetworkSimulationBuffer>();
            _messagesQuery = GetEntityQuery(
                ComponentType.ReadOnly<WrittenMessage>(), 
                ComponentType.ReadOnly<MessageTarget>());
        }

        protected override void OnUpdate()
        {
            var targetIndexesMap = new NativeHashMap<int, int>(1, Allocator.TempJob);
            var targetMessagesCount = new NativeList<int>(1, Allocator.TempJob);
            var targets = new NativeList<MessageTarget>(1, Allocator.TempJob);

            ComponentTypeHandle<MessageTarget> targetHandle = GetComponentTypeHandle<MessageTarget>();
            
            var gatherTargetsJob = new GatherTargetsJob
            {
                TargetHandle = targetHandle,
                TargetIndexesMap = targetIndexesMap,
                TargetMessagesCount = targetMessagesCount,
                Targets = targets
            };

            var jaggedMessages = new NativeArray<NativeList<WrittenMessage>>(targets.Length, Allocator.TempJob);

            var setupJaggedMessagesArrayJob = new SetupJaggedMessagesArrayJob
            {
                TargetMessagesCount = targetMessagesCount,
                JaggedMessages = jaggedMessages
            };
            
            var gatherMessagesJob = new GatherMessagesJob
            {
                TargetHandle = targetHandle,
                MessageHandle = GetComponentTypeHandle<WrittenMessage>(),
                TargetIndexesMap = targetIndexesMap,
                JaggedMessages = jaggedMessages
            };

            var sortMessagesJob = new SortMessagesJob
            {
                JaggedMessages = jaggedMessages
            };
            
            var packMessagesJob = new PackMessagesJob
            {
                Targets = targets,
                TargetsMessages = jaggedMessages,
                CommandBuffer = _endSimulationBuffer.CreateCommandBuffer().AsParallelWriter()
            };
            
            JobHandle packDeps = GetSingleton<MessagesSendingDependency>().Handle;
            
            // Gather targets
            packDeps = gatherTargetsJob.Schedule(_messagesQuery, packDeps);
            packDeps = setupJaggedMessagesArrayJob.Schedule(packDeps);
            packDeps = targetMessagesCount.Dispose(packDeps);
            
            // Gather messages
            packDeps = gatherMessagesJob.Schedule(_messagesQuery, packDeps);
            packDeps = targetIndexesMap.Dispose(packDeps);

            // Sort messages
            packDeps = sortMessagesJob.Schedule(jaggedMessages.Length, 1, packDeps);
            
            // Pack messages
            packDeps = packMessagesJob.Schedule(targets.Length, 1, packDeps);
            packDeps = jaggedMessages.Dispose(packDeps);
            packDeps = targets.Dispose(packDeps);
            
            SetSingleton(new MessagesSendingDependency { Handle = packDeps });
        }
    }
}