using System;

namespace PeacefulHills.Bootstrap
{
    public interface IBootOverride : IBootControl
    {
        public Type Type { get; }
    }
}