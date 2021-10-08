using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PeacefulHills.Bootstrap.Worlds
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> FilterAssembly(this IEnumerable<Type> types, string pattern)
        {
            foreach (Type type in types)
            {
                if (MatchAssembly(type, pattern))
                {
                    yield return type;
                }
            }
        }
        
        public static bool MatchAssembly(this Type type, string pattern)
        {
            return Regex.IsMatch(type.Assembly.FullName, pattern);
        }
    }
}