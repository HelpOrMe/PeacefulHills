using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace PeacefulHills.Bootstrap
{
    public class BootstrapWorldDefault : BootstrapWorldBase
    {
        public override World Initialize()
        {
            var world = new World("Default world");

            List<Type> types = Systems.All().Types().ToList();

            world.AddSystems(types);
            world.Loop();

            World.DefaultGameObjectInjectionWorld = world;
            return world;
        }
    }
}