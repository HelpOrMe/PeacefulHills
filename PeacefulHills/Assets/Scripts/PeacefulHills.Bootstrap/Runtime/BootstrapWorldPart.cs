using System.Collections.Generic;
using Unity.Entities;

namespace PeacefulHills.Bootstrap
{
    public abstract class BootstrapWorldPart
    {
        public IReadOnlyList<SystemInfo> Systems;

        public abstract void Initialize(World world);
    }
}