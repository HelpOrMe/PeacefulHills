using System.Collections.Generic;
using Unity.Entities;

namespace PeacefulHills.Bootstrap
{
    public abstract class WorldBootstrapBase
    {
        public IReadOnlyList<SystemInfo> Systems;

        public abstract void Initialize();
    }
}