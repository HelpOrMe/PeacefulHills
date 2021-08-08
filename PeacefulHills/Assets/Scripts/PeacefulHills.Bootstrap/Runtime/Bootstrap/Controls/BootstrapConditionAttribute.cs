using System;

namespace PeacefulHills.Bootstrap.Controls
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, 
                    Inherited = true, AllowMultiple = true)]
    public abstract class BootstrapConditionAttribute : Attribute
    {
        public abstract bool Check(WorldBootstrapInfo bootstrapInfo);
    }
}