﻿using PeacefulHills.Bootstrap;
using Unity.Entities;

namespace PeacefulHills.Network
{
    public class NetworkWorldBootstrap : BootstrapWorldBase
    {
        public override World Initialize()
        {
            var world = new World("Network world");
            world.AddSystems(Systems.AllTree().Types());
            world.Loop();
            return world;
        }
    }
}