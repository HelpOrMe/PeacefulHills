using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public class SystemInfo
    {
        public readonly List<SystemInfo> NestedSystems;
        public readonly Type Type;

        public SystemInfo(Type type, List<SystemInfo> nestedSystems)
        {
            Type = type;
            NestedSystems = nestedSystems;
        }
    }
}