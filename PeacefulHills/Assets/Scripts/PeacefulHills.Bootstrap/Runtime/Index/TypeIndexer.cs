using System;
using System.Collections.Generic;
using System.Reflection;

namespace PeacefulHills.Bootstrap.Index
{
    public static class TypeIndexer
    {
        public static Type[] IndexInherited<T>() where T : class
        {
            var types = new List<Type>();
            IndexInherited<T>(types);
            return types.ToArray();
        }
        
        public static void IndexInherited<T>(List<Type> output) where T : class
        {
            IndexInherited<T>(AppDomain.CurrentDomain, output);
        }
        
        public static void IndexInherited<T>(AppDomain domain, List<Type> output) where T : class
        {
            Index(domain, t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract, output);
        }
        
        public static void IndexInherited<T>(Assembly assembly, List<Type> output) where T : class
        {
            Index(assembly, t => typeof(T).IsAssignableFrom(t) && !t.IsAbstract, output);
        }
        
        public static IEnumerable<Type> Index(Func<Type, bool> filter)
        {
            var types = new List<Type>();
            Index(AppDomain.CurrentDomain, filter, types);
            return types.ToArray();
        }
        
        public static void Index(Func<Type, bool> filter, List<Type> output)
        {
            Index(AppDomain.CurrentDomain, filter, output);
        }
        
        public static void Index(AppDomain domain, Func<Type, bool> filter, List<Type> output)
        {
            foreach (Assembly assembly in domain.GetAssemblies())
            {
                if (assembly.GetCustomAttribute<BootstrapIndexAssemblyAttribute>() != null)
                {
                    Index(assembly, filter, output);
                }
            }
        }

        public static void Index(Assembly assembly, Func<Type, bool> filter, List<Type> output)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttribute<NoBootstrapIndexAttribute>() == null 
                    && filter(type))
                {
                    output.Add(type);
                }
            }
        }
    }
}