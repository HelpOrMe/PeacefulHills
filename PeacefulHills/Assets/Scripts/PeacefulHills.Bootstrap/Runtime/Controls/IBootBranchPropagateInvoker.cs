using System.Collections.Generic;
using PeacefulHills.Bootstrap.Tree;

namespace PeacefulHills.Bootstrap
{
    public interface IBootBranchPropagateInvoker : IBootControl
    {
        void Invoke(IList<IBootBranch> branches);
    }
}