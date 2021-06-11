using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Bootstrap
{
    public class CustomBootstrap : ICustomBootstrap
    {
        public bool Initialize(string defaultWorldName)
        {
            IReadOnlyList<Type> systemTypes = TypeManager.GetSystems(WorldSystemFilterFlags.Default);
            IEnumerable<WorldInfo> bootstrapWorlds = GetBootstrapWorldsInfo();

            List<SystemInfo> groups = NestGroups(GatherGroupByTypes(systemTypes));
            Dictionary<Type, List<SystemInfo>> systemsByWorld = GatherSystemsByWorld(groups);

            foreach (WorldInfo worldInfo in bootstrapWorlds)
            {
                if (!systemsByWorld.ContainsKey(worldInfo.Type))
                {
                    Debug.LogWarning($"{worldInfo.Type.Name} does not contain any groups that refer to it.");
                    continue;
                }

                if (Activator.CreateInstance(worldInfo.Type) is BootstrapWorldBase bootstrapWorld)
                {
                    List<SystemInfo> worldSystems = systemsByWorld[worldInfo.Type];

                    bootstrapWorld.Systems = worldSystems;
                    World world = bootstrapWorld.Initialize();

                    foreach (Type worldPartType in worldInfo.PartTypes)
                    {
                        if (Activator.CreateInstance(worldPartType) is BootstrapWorldPart bootstrapWorldPart)
                        {
                            bootstrapWorldPart.Systems = worldSystems;
                            bootstrapWorldPart.Initialize(world);
                        }
                    }
                }
            }

            return true;
        }

        private IEnumerable<WorldInfo> GetBootstrapWorldsInfo()
        {
            var worldsMap = new Dictionary<Type, WorldInfo>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetCustomAttribute<BootstrapAssemblyIndexAttribute>() == null)
                {
                    continue;
                }

                Type[] types = assembly.GetTypes();

                WriteBootstrapWorldTypes(types, worldsMap);
                WriteBootstrapWorldPartTypes(types, worldsMap);
            }

            return worldsMap.Values;
        }

        private void WriteBootstrapWorldTypes(IEnumerable<Type> types, Dictionary<Type, WorldInfo> worldsMap)
        {
            foreach (Type type in types)
            {
                if (typeof(BootstrapWorldBase).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    if (!worldsMap.ContainsKey(type))
                    {
                        worldsMap[type] = new WorldInfo(type, new List<Type>());
                    }
                }
            }
        }

        private void WriteBootstrapWorldPartTypes(IEnumerable<Type> types, Dictionary<Type, WorldInfo> worldsMap)
        {
            foreach (Type type in types)
            {
                if (typeof(BootstrapWorldPart).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    var attr = type.GetCustomAttribute<BootstrapWorldAttribute>();
                    if (attr == null)
                    {
                        Debug.LogError("BootstrapWorldPart must contain BootstrapWorldAttribute!");
                        continue;
                    }

                    worldsMap[attr.Type].PartTypes.Add(type);
                }
            }
        }

        private Dictionary<Type, SystemInfo> GatherGroupByTypes(IEnumerable<Type> systemTypes)
        {
            var systems = new Dictionary<Type, SystemInfo>();

            foreach (Type systemType in systemTypes)
            {
                Type groupType = systemType.GetCustomAttribute<UpdateInGroupAttribute>()?.GroupType;
                groupType ??= typeof(SimulationSystemGroup);

                var systemInfo = new SystemInfo(systemType, new List<SystemInfo>());

                if (!systems.ContainsKey(groupType))
                {
                    systems[groupType] = new SystemInfo(groupType, new List<SystemInfo>());
                }

                systems[groupType].NestedSystems.Add(systemInfo);
            }

            return systems;
        }

        private List<SystemInfo> NestGroups(Dictionary<Type, SystemInfo> groups)
        {
            foreach (Type groupType in groups.Keys)
            {
                Type parentGroupType = groupType.GetCustomAttribute<UpdateInGroupAttribute>()?.GroupType;
                if (parentGroupType != null)
                {
                    groups[parentGroupType].NestedSystems.Add(groups[groupType]);
                }
            }

            return new List<SystemInfo>
            {
                groups[typeof(InitializationSystemGroup)],
                groups[typeof(SimulationSystemGroup)],
                groups[typeof(PresentationSystemGroup)]
            };
        }

        private Dictionary<Type, List<SystemInfo>> GatherSystemsByWorld(List<SystemInfo> groups)
        {
            var systemsByWorld = new Dictionary<Type, List<SystemInfo>>
            {
                [typeof(BootstrapWorldDefault)] = new List<SystemInfo>()
            };

            foreach (SystemInfo group in groups)
            {
                foreach (SystemInfo system in group.NestedSystems)
                {
                    List<BootstrapWorldAttribute> updateInWorldAttrs = system.Type
                       .GetCustomAttributes<BootstrapWorldAttribute>()
                       .ToList();

                    if (updateInWorldAttrs.Count > 0)
                    {
                        foreach (BootstrapWorldAttribute worldAttr in updateInWorldAttrs)
                        {
                            Type worldType = worldAttr.Type;
                            if (!systemsByWorld.ContainsKey(worldType))
                            {
                                systemsByWorld[worldType] = new List<SystemInfo>();
                            }

                            systemsByWorld[worldType].Add(system);
                        }
                    }
                    else
                    {
                        systemsByWorld[typeof(BootstrapWorldDefault)].Add(system);
                    }
                }
            }

            return systemsByWorld;
        }
    }
}