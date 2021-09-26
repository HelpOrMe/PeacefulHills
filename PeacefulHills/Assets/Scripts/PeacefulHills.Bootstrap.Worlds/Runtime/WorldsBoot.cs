using System;
using System.Collections.Generic;
using System.Reflection;
using PeacefulHills.Bootstrap.Tree;
using Unity.Entities;

namespace PeacefulHills.Bootstrap.Worlds
{
    public class WorldsBoot : IBoot, IBootBranchPropagateInvoker, IBootBranchHolder, 
        // Default worlds bootstrap
        ICustomBootstrap
    {
        public IBootBranch Branch { get; set; }
        
        // Disable standard invoke propagation
        public void Invoke(IList<IBootBranch> branches)
        {
        }

        public bool Initialize(string defaultWorldName)
        {
            GatherSystemInGroups(
                out Dictionary<Type, List<Type>> systemsInGroup, 
                out Dictionary<Type, Type> systemInGroup);

            GatherSystemsInWorld(systemsInGroup, systemInGroup, 
                out Dictionary<Type, List<Type>> systemsInWorld);
            
            foreach (IBootBranch child in Branch.Children)
            {
                Type childType = child.GetType();
                if (systemsInWorld.ContainsKey(childType) && child is IBootWorld bootWorld)
                {
                    bootWorld.SystemTypes = systemsInWorld[childType];
                }
            }

            return true;
        }

        private void GatherSystemInGroups(
            out Dictionary<Type, List<Type>> systemsInGroup, 
            out Dictionary<Type, Type> systemInGroup)
        {
            IReadOnlyList<Type> systemTypes = TypeManager.GetSystems(WorldSystemFilterFlags.Default);
            
            systemsInGroup = new Dictionary<Type, List<Type>>();
            systemInGroup = new Dictionary<Type, Type>();
            
            foreach (Type systemType in systemTypes)
            {
                bool isGroup = typeof(ComponentSystemGroup).IsAssignableFrom(systemType);
                var groupAttr = systemType.GetCustomAttribute<UpdateInGroupAttribute>();
                
                Type groupType = groupAttr?.GroupType;
                if (!isGroup && groupType == null)
                {
                    groupType = typeof(SimulationSystemGroup);
                }
                if (groupType == null)
                {
                    continue;
                }
                
                systemInGroup[systemType] = groupType;

                if (!systemsInGroup.ContainsKey(groupType))
                {
                    systemsInGroup[groupType] = new List<Type>();
                }
                systemsInGroup[groupType].Add(systemType);
            }
        }

        private void GatherSystemsInWorld(
            Dictionary<Type, List<Type>> systemsInGroup, 
            Dictionary<Type, Type> systemInGroup,
            out Dictionary<Type, List<Type>> systemsInWorld)
        {
            systemsInWorld = new Dictionary<Type, List<Type>>();
            
            foreach (KeyValuePair<Type, List<Type>> groupAndTypes in systemsInGroup)
            {
                Type parentGroup = groupAndTypes.Key;
                for (; systemInGroup.ContainsKey(parentGroup); parentGroup = systemInGroup[parentGroup])
                {
                }

                var updateInWorldAttr = parentGroup.GetCustomAttribute<UpdateInWorldAttribute>();
                Type world = updateInWorldAttr?.Type ?? typeof(DefaultWorldBoot);

                if (!systemsInWorld.ContainsKey(world))
                {
                    systemsInWorld[world] = new List<Type>();
                }
                
                systemsInWorld[world].AddRange(groupAndTypes.Value);
            }
        }
    }
}