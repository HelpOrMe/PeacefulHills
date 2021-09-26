using System;

namespace PeacefulHills.Bootstrap
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class BootAfter : Attribute, IBootAfter
    {
        public Type Type { get; }

        public BootAfter(Type type)
        {
            Type = type;
        }
    }
}