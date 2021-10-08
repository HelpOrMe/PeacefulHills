using System;

namespace PeacefulHills.Bootstrap.Worlds
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class UpdateInWorldAttribute : Attribute
    {
        public Type Type { get; }

        public UpdateInWorldAttribute(Type type)
        {
            if (!typeof(WorldBootstrap).IsAssignableFrom(type))
            {
                throw new ArgumentException(
                    $"Invalid {nameof(WorldBootstrap)} passed to {nameof(UpdateInWorldAttribute)}: {type.FullName}");
            }
            
            Type = type;
        }
    }
}