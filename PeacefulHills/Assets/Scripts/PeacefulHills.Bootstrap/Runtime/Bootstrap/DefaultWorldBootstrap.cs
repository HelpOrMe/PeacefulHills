using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace PeacefulHills.Bootstrap
{
    public class DefaultWorldBootstrap : WorldBootstrapBase
    {
        public override void Initialize()
        {
            var world = new World("Default world");

            List<Type> types = Systems.Tree().Types().ToList();

            world.AddSystems(types);
            world.Loop();

            World.DefaultGameObjectInjectionWorld = world;
        }
    }
}