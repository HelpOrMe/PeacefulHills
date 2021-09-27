using System;

namespace PeacefulHills.Bootstrap
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class BootOverride : Attribute, IBootOverride
    {
        public Type Type { get; }

        public BootOverride(Type type)
        {
            Type = type;
        }
    }
}