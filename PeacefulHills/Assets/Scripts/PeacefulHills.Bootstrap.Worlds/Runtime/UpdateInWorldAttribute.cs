using System;

namespace PeacefulHills.Bootstrap.Worlds
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateInWorldAttribute : Attribute
    {
        public Type Type { get; }

        public UpdateInWorldAttribute(Type type)
        {
            if (!typeof(IBootWorld).IsAssignableFrom(type))
            {
                throw new ArgumentException(
                    $"Invalid {nameof(IBootWorld)} passed to {nameof(UpdateInWorldAttribute)}: {type.FullName}");
            }
            
            Type = type;
        }
    }
}