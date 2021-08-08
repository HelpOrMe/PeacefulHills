using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PeacefulHills.Bootstrap
{
    public class TypesIndexer : ITypesIndexer
    {
        private readonly List<Type> _types = new List<Type>();
        
        public void Index()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.GetCustomAttribute<BootstrapAssemblyIndexAttribute>() == null)
                {
                    continue;
                }
                
                foreach (Type type in assembly.GetTypes())
                {
                    _types.Add(type);
                }
            }
        }

        public IEnumerable<Type> Gather(Func<Type, bool> condition)
        {
            return _types.Where(condition);
        }
    }
}