using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public readonly struct WorldInfo
    {
        public readonly Type Type;
        public readonly Type BaseType;
        public readonly List<Type> PartTypes;

        public WorldInfo(Type type, Type baseType, List<Type> partTypes)
        {
            Type = type;
            PartTypes = partTypes;
            BaseType = baseType;
        }
    }
}