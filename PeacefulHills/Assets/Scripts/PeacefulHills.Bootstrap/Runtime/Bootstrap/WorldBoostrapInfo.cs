using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public readonly struct WorldBoostrapInfo
    {
        public readonly Type Type;
        public readonly Type BaseType;

        public WorldBoostrapInfo(Type type, Type baseType)
        {
            Type = type;
            BaseType = baseType;
        }
    }
}