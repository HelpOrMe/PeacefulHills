using System.Collections.Generic;

namespace PeacefulHills.Bootstrap.Tree
{
    public class BootBranch : IBootBranch
    {
        public BootInfo Boot { get; }
        public IList<IBootBranch> Children { get; } = new List<IBootBranch>();
        
        protected BootBranch(BootInfo boot)
        {
            Boot = boot;
        }

        public static IBootBranch Build(BootInfo boot)
        {
            boot.RequestControl<IBootInfoModifier>(mod => mod.ModifyInfo(ref boot));
            boot.RequestControl<IBootInfoHolder>(holder => holder.Boot = boot);

            IBootBranch branch = new BootBranch(boot);
            
            boot.RequestControl<IBootBranchModifier>(mod => mod.ModifyBranch(ref branch));
            boot.RequestControl<IBootBranchHolder>(holder => holder.Branch = branch);
            
            return branch;
        }

        public void PropagateInvoke()
        {
            if (Boot.TryGetControl(out IBootBranchInvoker branchInvoker))
            {
                branchInvoker.Invoke(Boot);
            }
            else if (Boot.Operand is IBootstrap bootstrap)
            {
                bootstrap.Invoke();
            }

            if (Boot.TryGetControl(out IBootBranchPropagateInvoker propagateInvoker))
            {
                propagateInvoker.Invoke(Children);
            }
            else
            {
                foreach (IBootBranch child in Children)
                {
                    child.PropagateInvoke();
                }
            }
        }

        public IBootBranch Clone()
        {
            IBootBranch clone = Build(Boot);
            
            foreach (IBootBranch child in Children)
            {
                clone.Children.Add(child);
            }

            return clone;
        }
    }
}