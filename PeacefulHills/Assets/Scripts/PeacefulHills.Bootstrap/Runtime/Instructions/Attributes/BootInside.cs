using System;

namespace PeacefulHills.Bootstrap
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class BootInside : Attribute, IBootInstruction
    {
        public readonly Type Type;

        public BootInside(Type type)
        {
            Type = type;
        }

        public void Apply(Boot boot)
        {
        }
    }
}