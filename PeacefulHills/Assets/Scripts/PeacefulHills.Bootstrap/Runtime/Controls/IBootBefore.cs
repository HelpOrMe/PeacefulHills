using System;

namespace PeacefulHills.Bootstrap
{
    public interface IBootBefore : IBootControl
    {
        public Type Type { get; }
    }
}