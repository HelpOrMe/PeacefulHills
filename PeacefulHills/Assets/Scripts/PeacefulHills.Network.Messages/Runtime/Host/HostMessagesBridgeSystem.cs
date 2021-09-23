#if !UNITY_SERVER || UNITY_EDITOR

using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    /// <summary>
    /// All packets that was sent to a connection with <see cref="HostConnection"/>
    /// tag will be directly marked for receiving without serialization and sending.
    ///
    /// If the packet was sent to all connections (broadcasting),
    /// a copy will be created to receive and the host connection
    /// will be ignored.
    /// </summary>
    
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
               .WithName("Host_messages_bridge")
               .WithAll<MessageSendRequest>()
               .ForEach((Entity messageEntity, ref MessageTarget target) =>
                {
                    // Check for broadcasting
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