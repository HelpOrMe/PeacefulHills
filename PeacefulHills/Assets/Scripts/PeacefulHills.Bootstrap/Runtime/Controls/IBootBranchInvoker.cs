using PeacefulHills.Bootstrap.Tree;

namespace PeacefulHills.Bootstrap
{
    public interface IBootBranchInvoker : IBootControl
    {
        void Invoke(BootInfo info);
    }
}