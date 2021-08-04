#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using PeacefulHills.Bootstrap;
using Unity.Entities;

namespace PeacefulHills.Network.Profiling
{
    public class SplitNetworkWorldBootstrap : NetworkWorldBootstrap
    {
        public override World Initialize()
        {
            WorldsInitializationSettings settings = WorldsInitializationSettings.Load();

            if (!settings.hostWorld)
            {
                List<SystemInfo> allGroups = Systems.AllTree().ToList();
                List<Type> clients = allGroups.MatchAssembly(@"\.(Shared|Client),").Types().ToList();
                List<Type> servers = allGroups.MatchAssembly(@"\.(Shared|Server),").Types().ToList();

                for (int i = 0; i < settings.clientCount; i++)
                {
                    InitializeSideWorld($"Client world №{i + 1}", clients);
                }

                return InitializeSideWorld("Server world", servers);
            }

            return InitializeNetworkWorld();
        }

        private World InitializeSideWorld(string worldName, IEnumerable<Type> systems)
        {
            var world = new World(worldName);
            world.AddSystems(systems);
            world.Loop();
            return world;
        }

        private World InitializeNetworkWorld()
        {
            var world = new World("Network world");
            world.AddSystems(Systems.AllTree().Types());
            world.Loop();
            return world;
        }
    }
}

#endif