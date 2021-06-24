#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using PeacefulHills.Bootstrap;
using PeacefulHills.ECS.World;
using Unity.Entities;

namespace PeacefulHills.Network.Profiling
{
    public class ProfiledNetworkWorldBootstrap : NetworkWorldBootstrap
    {
        public override World Initialize()
        {
            IProfilingSettings settings = ProfilingSettings.Load();
            
            if (settings.SeparateWorlds)
            {
                List<SystemInfo> allGroups = Systems.AllTree().ToList();
                List<Type> clients = allGroups.MatchAssembly(@"\.(Shared|Client),").Types().ToList();
                List<Type> servers = allGroups.MatchAssembly(@"\.(Shared|Server),").Types().ToList();
 
                for (int i = 0; i < settings.ClientCount; i++)
                {
                    InitializeSideWorld($"Client world №{i + 1}", clients, settings);
                }
                
                return InitializeSideWorld("Server world", servers, settings);
            }

            return StandardWorldInitialization();
        }

        private World InitializeSideWorld(string worldName, IEnumerable<Type> systems, IProfilingSettings settings)
        {
            var world = new World(worldName);
            world.AddSystems(systems);
            world.Loop();
            world.SetExtension(settings);
            return world;
        }
        
        private World StandardWorldInitialization()
        {
            var world = new World("Network world");
            world.AddSystems(Systems.AllTree().Types());
            world.Loop();
            return world;
        }
    }
}

#endif