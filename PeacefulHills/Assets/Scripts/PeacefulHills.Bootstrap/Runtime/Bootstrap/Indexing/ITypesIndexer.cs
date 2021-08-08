using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public interface ITypesIndexer
    {
        void Index();
        
        IEnumerable<Type> Gather(Func<Type, bool> condition);
    }
}