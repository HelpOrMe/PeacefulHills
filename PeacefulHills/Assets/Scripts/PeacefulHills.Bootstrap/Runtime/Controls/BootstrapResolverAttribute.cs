using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap.Controls
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public abstract class BootstrapResolverAttribute : Attribute
    {
        public abstract WorldBootstrapInfo Resolve(IEnumerable<WorldBootstrapInfo> bootstrapsInfo);
    }
}