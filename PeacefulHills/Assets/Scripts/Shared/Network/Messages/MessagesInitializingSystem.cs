using PeacefulHills.ECS.World;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public class MessagesInitializingSystem : SystemBase
    {
        protected override void OnCreate()
        {
            World.SetExtension<IMessagesRegistry>(new MessagesRegistry());
        }

        protected override void OnDestroy()
        {
            World.GetExtension<IMessagesRegistry>().Dispose();
        }

        protected override void OnUpdate()
        {
        }
    }
}