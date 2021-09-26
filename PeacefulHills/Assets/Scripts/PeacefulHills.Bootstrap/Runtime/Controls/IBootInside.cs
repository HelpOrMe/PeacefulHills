using System;

namespace PeacefulHills.Bootstrap
{
    public interface IBootInside : IBootControl
    {
        public Type Type { get; }
    }
}