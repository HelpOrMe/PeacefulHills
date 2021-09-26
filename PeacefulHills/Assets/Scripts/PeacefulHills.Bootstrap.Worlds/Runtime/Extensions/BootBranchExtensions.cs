using PeacefulHills.Bootstrap.Tree;
using Unity.Entities;

namespace PeacefulHills.Bootstrap.Worlds
{
    public static class BootBranchExtensions
    {
        public static void PropagateWorld(this IBootBranch root, World world)
        {
            root.ProcessTree(branch =>
            {
                if (branch is IBootWorldHolder holder)
                {
                    holder.World = world;
                }
            });
        }
    }
}