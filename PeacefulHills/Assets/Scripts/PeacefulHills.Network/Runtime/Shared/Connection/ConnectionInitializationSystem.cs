using PeacefulHills.Extensions;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    [UpdateAfter(typeof(NetworkInitializationSystem))]
    public class ConnectionInitializationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            Debug.Log("Connection init");
            World.SetExtension(new ConnectionBuilder(Allocator.Persistent));
        }

        protected override void OnDestroy()
        {
            World.GetExtension<ConnectionBuilder>().Dispose();
        }

        protected override void OnUpdate()
        {
        }
    }
}