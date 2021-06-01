using PeacefulHills.Bootstrap;
using PeacefulHills.ECS;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [BootstrapWorld(typeof(NetworkWorldBootstrap))]
    public class MessagesNetworkBootstrapPart : BootstrapWorldPart
    {
        public override void Initialize(World world)
        {
            world.SetExtension<IMessagesRegistry>(new MessagesRegistry());
        }
    }
}