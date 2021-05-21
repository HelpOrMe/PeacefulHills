using System;

namespace PeacefulHills.Bootstrap
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateInBootstrapWorldAttribute : Attribute
    {
        public readonly Type Type;

        public UpdateInBootstrapWorldAttribute(Type type)
        {
            Type = type;
        }
    }
}