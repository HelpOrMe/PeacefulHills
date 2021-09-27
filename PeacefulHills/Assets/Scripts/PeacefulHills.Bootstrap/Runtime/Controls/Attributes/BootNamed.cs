using System;

namespace PeacefulHills.Bootstrap
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class BootNamed : Attribute, IBootNamed
    {
        public string Name { get; }

        public BootNamed(string name)
        {
            Name = name;
        }
    }
}