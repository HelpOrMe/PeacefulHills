using Unity.Entities;

namespace PeacefulHills.Network.Send
{
    [UpdateInGroup(typeof(NetworkUpdateGroup))]
    public class ClientSendSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireSingletonForUpdate<NetworkSingleton>();
        }

        protected override void OnUpdate()
        {
            
        }
    }
}