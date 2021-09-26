using System.Collections.Generic;

namespace PeacefulHills.Bootstrap.Tree
{
    public interface IBootBranch
    {
        BootInfo Boot { get; }
        IList<IBootBranch> Children { get; }

        void PropagateInvoke();

        IBootBranch Clone();
    }
}