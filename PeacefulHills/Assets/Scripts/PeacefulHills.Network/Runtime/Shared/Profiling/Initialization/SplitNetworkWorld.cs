#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using PeacefulHills.Bootstrap;
using Unity.Entities;

namespace PeacefulHills.Network.Profiling
{
    [SplitWorldsCondition]
    public class SplitNetworkWorld : NetworkWorld
    {
        public override void Initialize()
        {
            INetworkWorldsInitSettings settings = NetworkWorldsInitSettings.Current;

            List<SystemTypeInfo> allGroups = Systems.Tree().ToList();
            List<Type> clients = allGroups.MatchAssembly(@"^(?!.*Server).*").Types().ToList();
            List<Type> servers = allGroups.MatchAssembly(@"^(?!.*Client).*").Types().ToList();

            for (int i = 0; i < settings.ClientCount; i++)
            {
                InitializeSideWorld($"Client world №{i + 1}", clients);
            }

            InitializeSideWorld("Server world", servers);
        }

        private World InitializeSideWorld(string worldName, IEnumerable<Type> systems)
        {
            var world = new World(worldName);
            world.AddSystems(systems);
            world.Loop();
            return world;
        }
    }
}

#endif