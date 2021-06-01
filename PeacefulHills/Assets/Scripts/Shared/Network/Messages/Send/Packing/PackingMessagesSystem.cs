using PeacefulHills.ECS.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    [UpdateAfter(typeof(WriteMessagesGroup))]
    public class PackingMessagesSystem : SystemBase
    {
        private EntityQuery _messagesQuery;
        private EndNetworkSimulationBuffer _endSimulationBuffer;
        
        protected override void OnCreate()
        {
            _endSimulationBuffer = World.GetOrCreateSystem<EndNetworkSimulationBuffer>();
            // _messagesQuery = GetEntityQuery(
                // ComponentType.ReadOnly<WrittenMessage>(), 
                // ComponentType.ReadOnly<MessageTarget>());
        }

        protected override void OnUpdate()
        {
            var targetIndexesMap = new NativeHashMap<int, int>(1, Allocator.TempJob);
            var targetMessagesCount = new NativeList<int>(1, Allocator.TempJob);
            var targets = new NativeList<MessageTarget>(1, Allocator.TempJob);
            
            Entities
                .WithAll<WrittenMessage>()
                .WithStoreEntityQueryInField(ref _messagesQuery)
                .ForEach((in MessageTarget target) =>
                {
                    int targetConnectionId = target.Connection.InternalId;
                
                    if (!targetIndexesMap.TryGetValue(targetConnectionId, out int messageIndex))
                    {
                        targetIndexesMap[targetConnectionId] = targets.Length;
                        targetMessagesCount.Add(1);
                        targets.Add(target);
                    }
                    else
                    {
                        // ReSharper disable once AccessToDisposedClosure
                        targetMessagesCount[messageIndex]++;
                    }
                }).Run();
            
            var jaggedMessages = new NativeJaggedArray<WrittenMessage>(targets.Length, Allocator.TempJob);
            
            for (int i = 0; i < jaggedMessages.Length; i++)
            {
                jaggedMessages[i] = new NativeArray<WrittenMessage>(targetMessagesCount[i], Allocator.TempJob);
            }

            targetMessagesCount.Dispose();
            
            var targetTempIndices = new NativeArray<int>(targets.Length, Allocator.TempJob);
            EntityCommandBuffer.ParallelWriter parallelEcb = _endSimulationBuffer.CreateCommandBuffer().AsParallelWriter();
            
            var popMessagesJob = new PopMessagesJob
            {
                TargetHandle = GetComponentTypeHandle<MessageTarget>(),
                MessageHandle = GetComponentTypeHandle<WrittenMessage>(),
                EntityHandle = GetEntityTypeHandle(),
                CommandBuffer = parallelEcb,
                TargetIndexesMap = targetIndexesMap,
                TargetTempIndices = targetTempIndices,
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
                CommandBuffer = parallelEcb
            };
            
            JobHandle packDeps = Dependency;

            // Pop messages
            packDeps = popMessagesJob.Schedule(_messagesQuery, packDeps);
            packDeps = targetTempIndices.Dispose(packDeps);
            packDeps = targetIndexesMap.Dispose(packDeps);

            // Sort messages
            packDeps = sortMessagesJob.Schedule(jaggedMessages.Length, 1, packDeps);
            
            // Pack messages
            packDeps = packMessagesJob.Schedule(targets.Length, 1, packDeps);
            packDeps = jaggedMessages.Dispose(packDeps);
            packDeps = targets.Dispose(packDeps);
            
            Dependency = packDeps;
            
            _endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }
    }
}