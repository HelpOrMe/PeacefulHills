using PeacefulHills.ECS;
using Unity.Entities;

namespace PeacefulHills.Network.Messages.Test
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    public class MessagesRegistrySystem : SystemBase
    {
        protected override void OnCreate()
        {
            World.RequestExtension<IMessagesRegistry>(RegisterMessages);
        }

        protected void RegisterMessages(IMessagesRegistry registry)
        {
            registry.Register<TestMessage>();
        }
        
        protected override void OnUpdate()
        {
            
        }
    }
}