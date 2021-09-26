using System;

namespace PeacefulHills.Bootstrap
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class BootInside : Attribute, IBootInside
    {
        public Type Type { get; }

        public BootInside(Type type)
        {
            Type = type;
        }
    }
}