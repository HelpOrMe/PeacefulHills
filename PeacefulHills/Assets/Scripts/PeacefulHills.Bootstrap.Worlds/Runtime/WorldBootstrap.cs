using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap.Worlds
{
    [BootInside(typeof(WorldsBootstrap))]
    public abstract class WorldBootstrap : Bootstrap
    {
        public IReadOnlyList<Type> Systems { get; set; } = Array.Empty<Type>();
    }
}