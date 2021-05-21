using System;
using System.Collections.Generic;
using Unity.Entities;

namespace PeacefulHills.Bootstrap
{
    public static class WorldExtensions
    {
        public static void AddSystems(this World world, IEnumerable<SystemInfo> systems) =>
            world.AddSystems(systems.Types());

        public static void AddSystems(this World world, IEnumerable<Type> systems) =>
            DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(world, systems);
        
        public static void Loop(this World world) 
            => ScriptBehaviourUpdateOrder.AddWorldToCurrentPlayerLoop(world);
    }
}