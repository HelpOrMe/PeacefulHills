using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    public class MessagesSendingInitializationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            SetSingleton(new MessagesSendingDependency());
        }
    }
}