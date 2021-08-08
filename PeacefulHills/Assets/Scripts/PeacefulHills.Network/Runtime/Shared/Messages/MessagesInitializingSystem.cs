using PeacefulHills.Extensions;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(MessagesInitializationGroup))]
    public class MessagesInitializingSystem : SystemBase
    {
        protected override void OnCreate()
        {
            World.SetExtension<IMessageRegistry>(new MessageRegistry());
        }

        protected override void OnDestroy()
        {
            World.GetExtension<IMessageRegistry>().Dispose();
        }

        protected override void OnUpdate()
        {
        }
    }
}