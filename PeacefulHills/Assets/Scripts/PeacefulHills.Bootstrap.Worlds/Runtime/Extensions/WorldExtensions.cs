using System;
using System.Collections.Generic;
using Unity.Entities;

namespace PeacefulHills.Bootstrap.Worlds
{
    public static class WorldExtensions
    {
        public static void AddSystems(this World world, IEnumerable<Type> systems)
        {
            DefaultWorldInitialization.AddSystemsToRootLevelSystemGroups(world, systems);
        }

        public static void Loop(this World world)
        {
            ScriptBehaviourUpdateOrder.AddWorldToCurrentPlayerLoop(world);
        }
    }
}