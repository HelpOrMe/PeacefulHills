using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public readonly struct WorldInfo
    {
        public readonly Type Type;
        public readonly List<Type> PartTypes;

        public WorldInfo(Type type, List<Type> partTypes)
        {
            Type = type;
            PartTypes = partTypes;
        }
    }
}