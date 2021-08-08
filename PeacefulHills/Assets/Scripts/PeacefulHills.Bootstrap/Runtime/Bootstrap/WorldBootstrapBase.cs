using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public abstract class WorldBootstrapBase
    {
        public IReadOnlyList<SystemTypeInfo> Systems;
        
        public abstract void Initialize();
    }
}