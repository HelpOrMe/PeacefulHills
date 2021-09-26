using System;
using System.Collections.Generic;
using PeacefulHills.Bootstrap.Tree;
using Unity.Entities;

namespace PeacefulHills.Bootstrap.Worlds
{
    [BootInside(typeof(WorldsBoot))]
    public class DefaultWorldBoot : IBootstrap, IBootWorld, IBootBranchHolder
    {
        public IReadOnlyList<Type> SystemTypes { get; set; }
        public IBootBranch Branch { get; set; }
        
        public void Invoke()
        {
            var world = new World("Default world");
            
            world.AddSystems(SystemTypes);
            world.Loop();

            World.DefaultGameObjectInjectionWorld = world;
            Branch.PropagateWorld(world);
        }
    }
}