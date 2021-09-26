using PeacefulHills.Bootstrap.Tree;

namespace PeacefulHills.Bootstrap
{
    public interface IBootBranchModifier : IBootControl
    {
        void ModifyBranch(ref IBootBranch branch);
    }
}