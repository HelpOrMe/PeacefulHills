using System;
using System.Collections.Generic;
using System.Reflection;

namespace PeacefulHills.Bootstrap.Index
{
    public static class TypeIndexer
    {
        /// <summary>
        /// Index types that implements TBase.
        /// </summary>
        public static IEnumerable<Type> IndexImplementation<TBase>()
        {
            return IndexImplementation(typeof(TBase));
        }
        
        /// <summary>
        /// Index types that implements <paramref name="targetBase"/>.
        /// </summary>
        public static IEnumerable<Type> IndexImplementation(Type targetBase)
        {
            foreach (Type type in Index())
            {
                if (!type.IsAbstract && targetBase.IsAssignableFrom(type))
                {
                    yield return type;
                }
            }
        }

        public static IEnumerable<Type> Index()
        {
            return Index(AppDomain.CurrentDomain);
        }
        
        public static IEnumerable<Type> Index(AppDomain domain)
        {
            foreach (Assembly assembly in domain.GetAssemblies())
            {
                if (assembly.GetCustomAttribute<BootstrapIndexAssemblyAttribute>() == null)
                {
                    continue;
                }
                
                foreach (Type type in assembly.GetTypes())
                {
                    yield return type;
                }
            }
        }

        public static IEnumerable<Type> Index(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                yield return type;
            }
        }
    }
}