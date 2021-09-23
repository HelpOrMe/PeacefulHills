using PeacefulHills.Extensions;
using PeacefulHills.Network.Packet;
using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(PacketRegistryInitializationGroup))]
    public class MessagePacketRegisterSystem : SystemBase
    {
        protected override void OnCreate()
        {
            Debug.Log("register");
            var registry = World.GetExtension<IPacketRegistry>();
            registry.RegisterNewPacketType(new ComponentTypes(typeof(MessagePacketAgent)));
        }
        
        protected override void OnUpdate()
        {
        }
    }
}