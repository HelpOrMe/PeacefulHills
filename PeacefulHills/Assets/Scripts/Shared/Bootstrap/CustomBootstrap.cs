using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;

namespace PeacefulHills.Bootstrap
{
    public class CustomBootstrap : ICustomBootstrap
    {
        public bool Initialize(string defaultWorldName)
        {
            IReadOnlyList<Type> systemTypes = TypeManager.GetSystems(WorldSystemFilterFlags.Default);
            IEnumerable<Type> bootstrapWorldTypes = GetBootstrapWorldTypes();
            
            List<SystemInfo> systems = GatherSystemsTree(systemTypes);
            Dictionary<Type, List<SystemInfo>> systemsByWorld = SortSystemsByWorld(systems);
            
            foreach (Type worldType in bootstrapWorldTypes)
            {
                if (Activator.CreateInstance(worldType) is BootstrapWorldBase bootstrapWorld)
                {
                    bootstrapWorld.Systems = systemsByWorld[worldType];
                    bootstrapWorld.Initialize();
                }
            }
            
            return true;
        }
    
        private IEnumerable<Type> GetBootstrapWorldTypes()
        {
            var worldTypes = new List<Type>();
            
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetCustomAttribute<BootstrapAssemblyIndexAttribute>() == null)
                {
                    continue;
                }
                
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(BootstrapWorldBase).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        worldTypes.Add(type);
                    }
                }
            }

            return worldTypes;
        }

        private List<SystemInfo> GatherSystemsTree(IEnumerable<Type> systemTypes)
        {
            var systems = new List<SystemInfo>
            {
                new SystemInfo(typeof(InitializationSystemGroup), new List<SystemInfo>()),
                new SystemInfo(typeof(SimulationSystemGroup), new List<SystemInfo>()),
                new SystemInfo(typeof(PresentationSystemGroup), new List<SystemInfo>()),
            };

            var groupsMap = new Dictionary<Type, int>
            {
                [typeof(InitializationSystemGroup)] = 0,
                [typeof(SimulationSystemGroup)] = 1,
                [typeof(PresentationSystemGroup)] = 2,
            };
            
            foreach (Type systemType in systemTypes)
            {
                List<UpdateInGroupAttribute> attrs = systemType.GetCustomAttributes<UpdateInGroupAttribute>().ToList();
                var systemInfo = new SystemInfo(systemType, new List<SystemInfo>());
                
                if (attrs.Count > 0)
                {
                    foreach (UpdateInGroupAttribute updateInGroupAttr in attrs)
                    {
                        if (!groupsMap.ContainsKey(updateInGroupAttr.GroupType))
                        {
                            groupsMap[updateInGroupAttr.GroupType] = systems.Count;
                            systems.Add(new SystemInfo(updateInGroupAttr.GroupType, new List<SystemInfo>()));
                        }
                        systems[groupsMap[updateInGroupAttr.GroupType]].NestedSystems.Add(systemInfo);
                    }
                }
                else
                {
                    systems[1].NestedSystems.Add(systemInfo);
                }
            }

            return systems;
        }

        private Dictionary<Type, List<SystemInfo>> SortSystemsByWorld(List<SystemInfo> systems)
        {
            var systemsByWorld = new Dictionary<Type, List<SystemInfo>>
            {
                [typeof(BootstrapWorldDefault)] = new List<SystemInfo>()
            };

            foreach (SystemInfo system in systems)
            {
                List<UpdateInBootstrapWorldAttribute> updateInWorldAttrs = system.Type
                    .GetCustomAttributes<UpdateInBootstrapWorldAttribute>()
                    .ToList();

                if (updateInWorldAttrs.Count > 0)
                {
                    foreach (var updateInWorldAttr in updateInWorldAttrs)
                    {
                        Type worldType = updateInWorldAttr.Type;
                        if (!systemsByWorld.ContainsKey(worldType))
                        {
                            systemsByWorld[worldType] = new List<SystemInfo> { system };
                        }
                        else
                        {
                            systemsByWorld[worldType].Add(system);
                        }
                    }
                }
                else
                {
                    systemsByWorld[typeof(BootstrapWorldDefault)].Add(system);
                }
            }

            return systemsByWorld;
        }
    }
}
