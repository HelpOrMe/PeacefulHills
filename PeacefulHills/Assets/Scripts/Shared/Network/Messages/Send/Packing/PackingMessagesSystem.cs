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
            _messagesQuery = GetEntityQuery(
                ComponentType.ReadOnly<WrittenMessage>(), 
                ComponentType.ReadOnly<MessageTarget>());
        }

        protected override void OnUpdate()
        {
            Dependency = PackMessages(SortMessages(PopMessages(GatherTargets(Dependency))));
            _endSimulationBuffer.AddJobHandleForProducer(Dependency);
        }

        private GatherTargetsStage GatherTargets(JobHandle dependency)
        {
            var targetIndexesMap = new NativeHashMap<int, int>(1, Allocator.TempJob);
            var targetMessagesCount = new NativeList<int>(1, Allocator.TempJob);
            var targets = new NativeList<MessageTarget>(1, Allocator.TempJob);

            Entities
                .WithAll<WrittenMessage>()
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
                        targetMessagesCount[messageIndex]++;
                    }
                })
                .Run();
            
            // TODO: Replace the jaggedArray constructor with the scheduling of a jaggedList with the Temp allocator
            var emptyJaggedMessages = new NativeJaggedArray<WrittenMessage>(targets.Length, Allocator.TempJob);
            
            for (int i = 0; i < emptyJaggedMessages.Length; i++)
            {
                emptyJaggedMessages[i] = new NativeArray<WrittenMessage>(targetMessagesCount[i], Allocator.TempJob);
            }
            
            return new GatherTargetsStage
            {
                Dependency = dependency,
                TargetIndexesMap = targetIndexesMap,
                Targets = targets,
                EmptyJaggedMessages = emptyJaggedMessages
            };
        }

        private PopMessagesStage PopMessages(GatherTargetsStage gatherTargetsStage)
        {
            JobHandle dependency = gatherTargetsStage.Dependency;
            NativeJaggedArray<WrittenMessage> jaggedMessages = gatherTargetsStage.EmptyJaggedMessages;
            
            var targetTempIndices = new NativeArray<int>(gatherTargetsStage.Targets.Length, Allocator.TempJob);
            var commandBuffer = _endSimulationBuffer.CreateCommandBuffer().AsParallelWriter();
            
            var popMessagesJob = new PopMessagesJob
            {
                TargetHandle = GetComponentTypeHandle<MessageTarget>(),
                MessageHandle = GetComponentTypeHandle<WrittenMessage>(),
                EntityHandle = GetEntityTypeHandle(),
                CommandBuffer = commandBuffer,
                TargetIndexesMap = gatherTargetsStage.TargetIndexesMap,
                TargetTempIndices = targetTempIndices,
                JaggedMessages = jaggedMessages
            };
            
            dependency = popMessagesJob.Schedule(_messagesQuery, dependency);
            dependency = targetTempIndices.Dispose(dependency);
            dependency = gatherTargetsStage.TargetIndexesMap.Dispose(dependency);

            return new PopMessagesStage
            {
                Dependency = dependency,
                Targets = gatherTargetsStage.Targets,
                JaggedMessages = jaggedMessages
            };
        }

        private PopMessagesStage SortMessages(PopMessagesStage popMessagesStage)
        {
            NativeJaggedArray<WrittenMessage> jaggedMessages = popMessagesStage.JaggedMessages;
            JobHandle dependency = popMessagesStage.Dependency;
            
            var sortMessagesJob = new SortMessagesJob
            {
                JaggedMessages = jaggedMessages
            };
            dependency = popMessagesStage.Dependency = sortMessagesJob.Schedule(jaggedMessages.Length, 1, dependency);

            popMessagesStage.Dependency = dependency;
            return popMessagesStage;
        }
        
        private JobHandle PackMessages(PopMessagesStage popMessagesStage)
        {
            JobHandle dependency = popMessagesStage.Dependency;
            NativeList<MessageTarget> targets = popMessagesStage.Targets;
            NativeJaggedArray<WrittenMessage> jaggedMessages = popMessagesStage.JaggedMessages;
            
            var commandBuffer = _endSimulationBuffer.CreateCommandBuffer().AsParallelWriter();
            
            var packMessagesJob = new PackMessagesJob
            {
                Targets = targets,
                TargetsMessages = jaggedMessages,
                CommandBuffer = commandBuffer
            };
            
            dependency = packMessagesJob.Schedule(targets.Length, 1, dependency);
            dependency = jaggedMessages.Dispose(dependency);
            dependency = targets.Dispose(dependency);

            return dependency;
        }
        
        public struct GatherTargetsStage
        {
            public JobHandle Dependency;
            
            public NativeHashMap<int, int> TargetIndexesMap;
            public NativeList<MessageTarget> Targets;
            public NativeJaggedArray<WrittenMessage> EmptyJaggedMessages;
        }
        
        public struct PopMessagesStage
        {
            public JobHandle Dependency;
            
            public NativeList<MessageTarget> Targets;
            public NativeJaggedArray<WrittenMessage> JaggedMessages;
        }
    }
}