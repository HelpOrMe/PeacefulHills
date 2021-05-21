using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public readonly struct SystemInfo
    {
        public readonly Type Type;
        public readonly List<SystemInfo> NestedSystems;

        public SystemInfo(Type type, List<SystemInfo> nestedSystems)
        {
            Type = type;
            NestedSystems = nestedSystems;
        }
    }
}