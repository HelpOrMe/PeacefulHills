using PeacefulHills.Extensions;
using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Network.Packet
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    [UpdateAfter(typeof(PacketPreInitializationSystem))]
    public class PacketPostInitializationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            Debug.Log("Packet post init");

            var registry = World.GetExtension<IPacketRegistry>();
            var connectionBuilder = World.GetExtension<ConnectionBuilder>();
            
            foreach (PacketType packet in registry.Packets)
            {
                connectionBuilder.AddPacket(packet);
            }
        }

        protected override void OnUpdate()
        {
        }
    }
}