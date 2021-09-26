using System;

namespace PeacefulHills.Bootstrap.Index
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class NoBootstrapIndexAttribute : Attribute
    {
        
    }
}