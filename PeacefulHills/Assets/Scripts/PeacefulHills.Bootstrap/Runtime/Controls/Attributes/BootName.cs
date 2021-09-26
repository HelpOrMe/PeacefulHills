using System;

namespace PeacefulHills.Bootstrap
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class BootName : Attribute, IBootName
    {
        public string Name { get; }

        public BootName(string name)
        {
            Name = name;
        }
    }
}