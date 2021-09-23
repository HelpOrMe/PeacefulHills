using PeacefulHills.Extensions;
using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Network.Packet
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    [UpdateAfter(typeof(PacketRegistryInitializationGroup))]
    public class PacketPreInitializationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            Debug.Log("Packet pre init");
            World.SetExtension<IPacketRegistry>(new PacketRegistry());
        }

        protected override void OnUpdate()
        {
        }
    }
}