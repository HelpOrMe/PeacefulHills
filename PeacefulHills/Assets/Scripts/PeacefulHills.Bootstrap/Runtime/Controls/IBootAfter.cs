using System;

namespace PeacefulHills.Bootstrap
{
    public interface IBootAfter : IBootControl
    {
        public Type Type { get; }
    }
}