using System;

namespace PeacefulHills.Bootstrap
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class WorldBootstrapAttribute : Attribute
    {
        public readonly Type Type;

        public WorldBootstrapAttribute(Type type)
        {
            if (!typeof(WorldBootstrapBase).IsAssignableFrom(type))
            {
                throw new ArgumentException("Invalid bootstrap world type " + type.Name);
            }

            Type = type;
        }
    }
}