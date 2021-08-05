using System;

namespace PeacefulHills.Bootstrap
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class BootstrapWorldAttribute : Attribute
    {
        public readonly Type Type;

        public BootstrapWorldAttribute(Type type)
        {
            if (!typeof(BootstrapWorldBase).IsAssignableFrom(type))
            {
                throw new ArgumentException("Invalid bootstrap world type " + type.Name);
            }

            Type = type;
        }
    }
}