using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Bootstrap
{
    public class WorldBootstrapBuilder : IWorldBootstrapBuilder
    {
        public List<WorldBootstrapContext> BuildBootstrapContexts()
        {
            var bootstrapWorldContexts = new List<WorldBootstrapContext>();
            
            IReadOnlyList<Type> systemTypes = TypeManager.GetSystems(WorldSystemFilterFlags.Default);
            IEnumerable<WorldBoostrapInfo> worldBootstraps = GetWorldBootstrapsInfo();

            List<SystemInfo> groups = NestGroups(GatherGroupByTypes(systemTypes));
            Dictionary<Type, List<SystemInfo>> systemsByWorld = GatherSystemsByWorld(groups);

            foreach (WorldBoostrapInfo worldBootstrapInfo in worldBootstraps)
            {
                if (!systemsByWorld.ContainsKey(worldBootstrapInfo.BaseType))
                {
                    Debug.LogWarning($"{worldBootstrapInfo.Type.Name} does not contain any groups that refer to it.");
                    continue;
                }

                if (Activator.CreateInstance(worldBootstrapInfo.Type) is WorldBootstrapBase worldBootstrap)
                {
                    List<SystemInfo> worldSystems = systemsByWorld[worldBootstrapInfo.BaseType];
                    worldBootstrap.Systems = worldSystems;

                    bootstrapWorldContexts.Add(new WorldBootstrapContext
                    {
                        Bootstrap = worldBootstrap,
                        Info = worldBootstrapInfo
                    });
                }
            }

            return bootstrapWorldContexts;
        }
 
        private IEnumerable<WorldBoostrapInfo> GetWorldBootstrapsInfo()
        {
            var worldsMap = new Dictionary<Type, WorldBoostrapInfo>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetCustomAttribute<BootstrapAssemblyIndexAttribute>() == null)
                {
                    continue;
                }

                Type[] types = assembly.GetTypes();

                WriteBootstrapWorldTypes(types, worldsMap);
            }

            return worldsMap.Values;
        }

        private void WriteBootstrapWorldTypes(IEnumerable<Type> types, Dictionary<Type, WorldBoostrapInfo> worldsMap)
        {
            foreach (Type type in types)
            {
                if (!typeof(WorldBootstrapBase).IsAssignableFrom(type) || type.IsAbstract)
                {
                    continue;
                }

                Type worldBaseType = type;
                for (; worldBaseType!.BaseType != typeof(WorldBootstrapBase); worldBaseType = worldBaseType!.BaseType)
                {
                }
                
                if (!worldsMap.ContainsKey(worldBaseType) || worldsMap[worldBaseType].Type.IsAssignableFrom(type))
                {
                    worldsMap[worldBaseType] = new WorldBoostrapInfo(type, worldBaseType);
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
                [typeof(DefaultWorldBootstrap)] = new List<SystemInfo>()
            };

            foreach (SystemInfo group in groups)
            {
                foreach (SystemInfo system in group.NestedSystems)
                {
                    List<WorldBootstrapAttribute> updateInWorldAttrs = system.Type
                       .GetCustomAttributes<WorldBootstrapAttribute>()
                       .ToList();

                    if (updateInWorldAttrs.Count > 0)
                    {
                        foreach (WorldBootstrapAttribute worldAttr in updateInWorldAttrs)
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
                        systemsByWorld[typeof(DefaultWorldBootstrap)].Add(system);
                    }
                }
            }

            return systemsByWorld;
        }
    }
}