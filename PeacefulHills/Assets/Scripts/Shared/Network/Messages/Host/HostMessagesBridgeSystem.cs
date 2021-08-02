#if !UNITY_SERVER || UNITY_EDITOR

using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(MessagesSimulationGroup))]
    [UpdateBefore(typeof(MessagesWriteGroup))]
    public class HostMessagesBridgeSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<HostConnection>();
        }

        protected override void OnUpdate()
        {
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            
            Entities
               .WithName("Convert_host_messages")
               .WithAll<MessageSendRequest>()
               .ForEach((Entity messageEntity, ref MessageTarget target) =>
                {
                    if (target.Connection == Entity.Null)
                    {
                        messageEntity = commandBuffer.Instantiate(messageEntity);
                    }
                    
                    commandBuffer.RemoveComponent<MessageSendRequest>(messageEntity);
                    commandBuffer.AddComponent<MessageReceiveRequest>(messageEntity);
                })
               .WithoutBurst()
               .Run();
            
            commandBuffer.Playback(EntityManager);
        }
    }
}

#endif