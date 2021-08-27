using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Bootstrap
{
    public class WorldBootstrapBuilder : IWorldBootstrapsBuilder
    {
        private readonly ITypesIndexer _indexer = new TypesIndexer();
        
        public Dictionary<Type, WorldBootstrapInfo> Build()
        {
            _indexer.Index();
            
            Dictionary<Type, WorldBootstrapInfo> worldBootstraps = GetWorldBootstrapTypes();
            SetSystemsToWorldBootstraps(worldBootstraps);
            return worldBootstraps;
        }
 
        private Dictionary<Type, WorldBootstrapInfo> GetWorldBootstrapTypes()
        {
            Dictionary<Type, WorldBootstrapInfo> worldBootstrapTypes = FindWorldBootstrapTypes();
            LinkBootstrapWorldTypeOverloads(worldBootstrapTypes);
            
            return worldBootstrapTypes;
        }

        private Dictionary<Type, WorldBootstrapInfo> FindWorldBootstrapTypes()
        {
            var worldBootstrapTypes = new Dictionary<Type, WorldBootstrapInfo>();

            IEnumerable<Type> bootstrapTypes = _indexer.Gather(
                t => typeof(WorldBootstrapBase).IsAssignableFrom(t) && !t.IsAbstract);
            
            foreach (Type type in bootstrapTypes)
            {
                worldBootstrapTypes[type] = new WorldBootstrapInfo(type);
            }
            
            return worldBootstrapTypes;
        }

        private void LinkBootstrapWorldTypeOverloads(Dictionary<Type, WorldBootstrapInfo> worldBootstrapTypes)
        {
            foreach (Type worldBootstrapType in worldBootstrapTypes.Keys)
            {
                Type worldBootstrapBaseType = worldBootstrapType.BaseType;

                if (worldBootstrapBaseType == typeof(WorldBootstrapBase) 
                    || worldBootstrapBaseType!.IsAbstract 
                    || !worldBootstrapTypes.ContainsKey(worldBootstrapBaseType))
                {
                    continue;
                }

                WorldBootstrapInfo worldBootstrapBaseInfo = worldBootstrapTypes[worldBootstrapBaseType];
                WorldBootstrapInfo worldBootstrapInfo = worldBootstrapTypes[worldBootstrapType];
                worldBootstrapInfo.BaseBootstrap = worldBootstrapBaseInfo;
                worldBootstrapBaseInfo.OverloadBootstraps.Add(worldBootstrapInfo);
            }
        }
        
        private void SetSystemsToWorldBootstraps(Dictionary<Type, WorldBootstrapInfo> worldBootstraps)
        {
            IReadOnlyList<Type> systemTypes = TypeManager.GetSystems(WorldSystemFilterFlags.Default);
            List<SystemTypeInfo> groups = NestGroups(GatherGroupByTypes(systemTypes));
            Dictionary<Type, List<SystemTypeInfo>> systemsByWorld = GatherSystemsByWorld(groups);
            
            foreach (WorldBootstrapInfo worldBootstrapInfo in worldBootstraps.Values)
            {
                var bootstrapSystems = new List<SystemTypeInfo>();

                for (WorldBootstrapInfo baseWorldBootstrapInfo = worldBootstrapInfo;
                    baseWorldBootstrapInfo != null;
                    baseWorldBootstrapInfo = baseWorldBootstrapInfo.BaseBootstrap)
                {
                    if (systemsByWorld.ContainsKey(baseWorldBootstrapInfo.Type))
                    {
                        bootstrapSystems.AddRange(systemsByWorld[baseWorldBootstrapInfo.Type]);
                    }
                }

                if (bootstrapSystems.Count == 0)
                {
                    Debug.LogWarning($"{worldBootstrapInfo.Type.Name} does not contain any groups that refer to it.");
                    continue;
                }
                
                worldBootstrapInfo.Systems = bootstrapSystems;
            }
        }
        
        private Dictionary<Type, SystemTypeInfo> GatherGroupByTypes(IEnumerable<Type> systemTypes)
        {
            var systems = new Dictionary<Type, SystemTypeInfo>();

            foreach (Type systemType in systemTypes)
            {
                Type groupType = systemType.GetCustomAttribute<UpdateInGroupAttribute>()?.GroupType;
                groupType ??= typeof(SimulationSystemGroup);

                var systemInfo = new SystemTypeInfo(systemType, new List<SystemTypeInfo>());

                if (!systems.ContainsKey(groupType))
                {
                    systems[groupType] = new SystemTypeInfo(groupType, new List<SystemTypeInfo>());
                }

                systems[groupType].NestedSystems.Add(systemInfo);
            }

            return systems;
        }

        private List<SystemTypeInfo> NestGroups(Dictionary<Type, SystemTypeInfo> groups)
        {
            foreach (Type groupType in groups.Keys)
            {
                Type parentGroupType = groupType.GetCustomAttribute<UpdateInGroupAttribute>()?.GroupType;
                if (parentGroupType != null)
                {
                    groups[parentGroupType].NestedSystems.Add(groups[groupType]);
                }
            }

            return new List<SystemTypeInfo>
            {
                groups[typeof(InitializationSystemGroup)],
                groups[typeof(SimulationSystemGroup)],
                groups[typeof(PresentationSystemGroup)]
            };
        }

        private Dictionary<Type, List<SystemTypeInfo>> GatherSystemsByWorld(List<SystemTypeInfo> groups)
        {
            var systemsByWorld = new Dictionary<Type, List<SystemTypeInfo>>
            {
                [typeof(DefaultWorldBootstrap)] = new List<SystemTypeInfo>()
            };

            foreach (SystemTypeInfo group in groups)
            {
                foreach (SystemTypeInfo system in group.NestedSystems)
                {
                    ReferSystemToWorld(system, systemsByWorld);
                }
            }

            return systemsByWorld;
        }

        private void ReferSystemToWorld(SystemTypeInfo systemType, Dictionary<Type, List<SystemTypeInfo>> systemsByWorld)
        {
            List<UpdateInWorldAttribute> updateInWorldAttrs = systemType.Type
               .GetCustomAttributes<UpdateInWorldAttribute>()
               .ToList();

            if (updateInWorldAttrs.Count > 0)
            {
                foreach (UpdateInWorldAttribute worldAttr in updateInWorldAttrs)
                {
                    Type worldType = worldAttr.Type;
                    if (!systemsByWorld.ContainsKey(worldType))
                    {
                        systemsByWorld[worldType] = new List<SystemTypeInfo>();
                    }

                    systemsByWorld[worldType].Add(systemType);
                }
            }
            else
            {
                systemsByWorld[typeof(DefaultWorldBootstrap)].Add(systemType);
            }
        }
    }
}