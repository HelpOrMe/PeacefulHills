using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.Entities;

namespace PeacefulHills.Bootstrap
{
    public static class SystemInfoExtensions
    {
        public static SystemTypeInfo Group<TGroup>(this IEnumerable<SystemTypeInfo> systems)
            where TGroup : ComponentSystemGroup
        {
            return systems.First(system => typeof(TGroup).IsAssignableFrom(system.Type));
        }

        public static IEnumerable<SystemTypeInfo> Groups<TGroup0, TGroup1>(this IEnumerable<SystemTypeInfo> systems)
            where TGroup0 : ComponentSystemGroup
            where TGroup1 : ComponentSystemGroup
        {
            return systems.Groups(new[] {typeof(TGroup0), typeof(TGroup1)});
        }

        public static IEnumerable<SystemTypeInfo> Groups<TGroup0, TGroup1, TGroup2>(this IEnumerable<SystemTypeInfo> systems)
            where TGroup0 : ComponentSystemGroup
            where TGroup1 : ComponentSystemGroup
            where TGroup2 : ComponentSystemGroup
        {
            return systems.Groups(new[] {typeof(TGroup0), typeof(TGroup1), typeof(TGroup2)});
        }

        public static IEnumerable<SystemTypeInfo> Groups(this IEnumerable<SystemTypeInfo> systems,
            IEnumerable<Type> targetGroups)
        {
            return systems.Where(system => targetGroups.Contains(system.Type));
        }

        public static IEnumerable<SystemTypeInfo> Groups(this IEnumerable<SystemTypeInfo> systems,
            IEnumerable<SystemTypeInfo> targetGroups)
        {
            return systems.Where(targetGroups.Contains);
        }

        public static IEnumerable<SystemTypeInfo> MatchAssembly(this IEnumerable<SystemTypeInfo> systems, string pattern)
        {
            return systems.Where(system => Regex.IsMatch(system.Type.Assembly.FullName, pattern));
        }
        
        public static IEnumerable<SystemTypeInfo> Nested(this IEnumerable<SystemTypeInfo> systems)
        {
            foreach (SystemTypeInfo system in systems)
            {
                foreach (SystemTypeInfo nestedSystem in system.NestedSystems)
                {
                    yield return nestedSystem;
                }
            }
        }

        public static IEnumerable<SystemTypeInfo> Tree(this IEnumerable<SystemTypeInfo> systems)
        {
            foreach (SystemTypeInfo system in systems)
            {
                foreach (SystemTypeInfo systemInfo in system.Tree())
                {
                    yield return systemInfo;
                }
            }
        }

        public static IEnumerable<SystemTypeInfo> Tree(this SystemTypeInfo systemType)
        {
            yield return systemType;

            foreach (SystemTypeInfo nestedSystem in systemType.NestedSystems)
            {
                foreach (SystemTypeInfo systemInfo in Tree(nestedSystem))
                {
                    yield return systemInfo;
                }
            }
        }

        public static IEnumerable<Type> Types(this IEnumerable<SystemTypeInfo> systems)
        {
            return systems.Select(system => system.Type);
        }
    }
}