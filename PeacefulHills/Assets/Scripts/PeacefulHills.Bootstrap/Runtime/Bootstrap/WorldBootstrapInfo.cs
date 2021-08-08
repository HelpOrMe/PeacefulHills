using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public class WorldBootstrapInfo
    {
        public Type Type;
        public List<WorldBootstrapInfo> OverloadBootstraps;
        public WorldBootstrapInfo BaseBootstrap;

        public List<SystemTypeInfo> Systems;

        public WorldBootstrapInfo(Type type)
        {
            Type = type;
            OverloadBootstraps = new List<WorldBootstrapInfo>();
            Systems = new List<SystemTypeInfo>();
        }
    }
}