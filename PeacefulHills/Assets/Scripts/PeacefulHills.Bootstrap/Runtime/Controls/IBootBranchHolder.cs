using PeacefulHills.Bootstrap.Tree;

namespace PeacefulHills.Bootstrap
{
    public interface IBootBranchHolder : IBootControl
    {
        public IBootBranch Branch { get; set; }
    }
}