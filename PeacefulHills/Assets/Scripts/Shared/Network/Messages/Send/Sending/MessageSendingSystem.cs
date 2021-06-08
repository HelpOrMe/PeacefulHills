using PeacefulHills.ECS;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    [UpdateBefore(typeof(EndNetworkSimulationBuffer))]
    public class MessageSendingSystem : SystemBase
    {
        protected override void OnCreate()
        {
            this.RequireExtension<INetwork>();
        }

        protected override unsafe void OnUpdate()
        {
            var network = World.GetExtension<INetwork>();

            var concurrentDriver = network.DriverConcurrent;
            
            network.DriverDependency = Entities
                .ForEach((Entity entity, in DynamicBuffer<OutputMessage> outputMessage, in MessageTarget target) =>
                {
                    if (concurrentDriver.BeginSend(target.Connection, out DataStreamWriter writer) == 0)
                    {
                        foreach (OutputMessage message in outputMessage)
                        {
                            writer.WriteBytes(message.Bytes, message.Size);
                        }
                        concurrentDriver.EndSend(writer);
                    }
                })
                .ScheduleParallel(network.DriverDependency);
            
            Dependency = network.DriverDependency;
        }
    }
}