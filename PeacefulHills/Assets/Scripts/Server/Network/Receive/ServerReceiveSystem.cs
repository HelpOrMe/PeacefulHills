﻿
using Unity.Entities;

namespace PeacefulHills.Network.Receive
{
    public class ServerReceiveSystem : SystemBase
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