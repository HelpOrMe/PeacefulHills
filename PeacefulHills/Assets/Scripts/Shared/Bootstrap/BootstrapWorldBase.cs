using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public abstract class BootstrapWorldBase
    {
        public IReadOnlyList<SystemInfo> Systems;

        public abstract void Initialize();
    }
}