using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public interface IWorldBootstrapsBuilder
    {
        Dictionary<Type, WorldBootstrapInfo> Build();
    }
}