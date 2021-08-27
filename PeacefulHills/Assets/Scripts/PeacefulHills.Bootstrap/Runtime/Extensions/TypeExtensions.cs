using System;
using System.Linq;
using System.Reflection;

namespace PeacefulHills.Bootstrap
{
    public static class TypeExtensions
    {
        public static bool HasAttribute<TAttribute>(this Type type) where TAttribute : class
        {
            return type.GetCustomAttributes(true).FirstOrDefault(a => a is TAttribute) != null;
        }
        
        public static bool TryGetAttribute<TAttribute>(this Type type, out TAttribute attribute) where TAttribute : class
        {
            attribute = (TAttribute)type.GetCustomAttributes(true).FirstOrDefault(a => a is TAttribute);
            return attribute != null;
        }
    }
}