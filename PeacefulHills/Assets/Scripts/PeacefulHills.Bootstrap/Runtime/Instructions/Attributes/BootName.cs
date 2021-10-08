using System;

namespace PeacefulHills.Bootstrap
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class BootName : Attribute, IBootInstruction
    {
        public readonly string Name;

        public BootName(string name)
        {
            Name = name;
        }

        public void Apply(Boot boot)
        {
        }
    }
}