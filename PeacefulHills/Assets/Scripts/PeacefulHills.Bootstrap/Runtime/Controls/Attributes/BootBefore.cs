using System;

namespace PeacefulHills.Bootstrap
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class BootBefore : Attribute, IBootBefore
    {
        public Type Type { get; }

        public BootBefore(Type type)
        {
            Type = type;
        }
    }
}