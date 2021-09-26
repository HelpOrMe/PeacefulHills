using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap.Worlds
{
    public interface IBootWorld : IBoot
    {
        IReadOnlyList<Type> SystemTypes { get; set; }
    }
}