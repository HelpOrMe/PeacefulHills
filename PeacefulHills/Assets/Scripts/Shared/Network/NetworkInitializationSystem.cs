﻿using PeacefulHills.ECS.World;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    [UpdateAfter(typeof(BeginNetworkInitializationBuffer))]
    public class NetworkInitializationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            var driver = NetworkDriver.Create();
            var network = new Network
            {
                Driver = driver,
                DriverConcurrent = driver.ToConcurrent()
            };
            
            World.SetExtension<INetwork>(network);
        }

        protected override void OnDestroy()
        {
            var network = World.GetExtension<INetwork>();
            network.Driver.Dispose();
        }

        protected override void OnUpdate()
        {
        }
    }
}