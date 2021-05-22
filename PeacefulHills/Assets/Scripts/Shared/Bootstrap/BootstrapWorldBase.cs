using System.Collections.Generic;
using Unity.Entities;

namespace PeacefulHills.Bootstrap
{
    public abstract class BootstrapWorldBase
    {
        public IReadOnlyList<SystemInfo> Systems;

        public abstract World Initialize();
    }
}