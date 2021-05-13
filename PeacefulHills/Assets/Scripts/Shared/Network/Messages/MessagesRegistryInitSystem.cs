using PeacefulHills.ECS;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public class MessagesRegistryInitSystem : SystemBase
    {
        protected override void OnCreate()
        {
            World.SetExtension<IMessagesRegistry>(new MessagesRegistry());
        }

        protected override void OnUpdate() { }
    }
}