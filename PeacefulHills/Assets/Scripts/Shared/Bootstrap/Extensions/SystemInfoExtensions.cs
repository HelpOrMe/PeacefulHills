using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;

namespace PeacefulHills.Bootstrap
{
    public static class SystemInfoExtensions
    {
        public static SystemInfo Group<TGroup>(this IEnumerable<SystemInfo> systems)
            where TGroup : ComponentSystemGroup
        {
            return systems.First(system => typeof(TGroup).IsAssignableFrom(system.Type));
        }

        public static IEnumerable<SystemInfo> Groups<TGroup0, TGroup1>(this IEnumerable<SystemInfo> systems)
            where TGroup0 : ComponentSystemGroup
            where TGroup1 : ComponentSystemGroup
        {
            return systems.Groups(new[] {typeof(TGroup0), typeof(TGroup1)});
        }

        public static IEnumerable<SystemInfo> Groups<TGroup0, TGroup1, TGroup2>(this IEnumerable<SystemInfo> systems)
            where TGroup0 : ComponentSystemGroup
            where TGroup1 : ComponentSystemGroup
            where TGroup2 : ComponentSystemGroup
        {
            return systems.Groups(new[] {typeof(TGroup0), typeof(TGroup1), typeof(TGroup2)});
        }

        public static IEnumerable<SystemInfo> Groups(this IEnumerable<SystemInfo> systems,
                                                     IEnumerable<Type> targetGroups)
        {
            return systems.Where(system => targetGroups.Contains(system.Type));
        }

        public static IEnumerable<SystemInfo> Groups(this IEnumerable<SystemInfo> systems,
                                                     IEnumerable<SystemInfo> targetGroups)
        {
            return systems.Where(targetGroups.Contains);
        }

        public static IEnumerable<SystemInfo> Nested(this IEnumerable<SystemInfo> systems)
        {
            foreach (SystemInfo system in systems)
            {
                foreach (SystemInfo nestedSystem in system.NestedSystems)
                {
                    yield return nestedSystem;
                }
            }
        }

        public static IEnumerable<SystemInfo> All(this IEnumerable<SystemInfo> systems)
        {
            foreach (SystemInfo system in systems)
            {
                foreach (SystemInfo systemInfo in system.All())
                {
                    yield return systemInfo;
                }
            }
        }

        public static IEnumerable<SystemInfo> All(this SystemInfo system)
        {
            yield return system;

            foreach (SystemInfo nestedSystem in system.NestedSystems)
            {
                foreach (SystemInfo systemInfo in All(nestedSystem))
                {
                    yield return systemInfo;
                }
            }
        }

        public static IEnumerable<Type> Types(this IEnumerable<SystemInfo> systems)
        {
            return systems.Select(system => system.Type);
        }
    }
}