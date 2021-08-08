using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public class SystemTypeInfo
    {
        public readonly List<SystemTypeInfo> NestedSystems;
        public readonly Type Type;

        public SystemTypeInfo(Type type, List<SystemTypeInfo> nestedSystems)
        {
            Type = type;
            NestedSystems = nestedSystems;
        }
    }
}