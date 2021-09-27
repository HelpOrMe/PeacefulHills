using System;
using System.Collections.Generic;
using System.Linq;

namespace PeacefulHills.Bootstrap.Tree
{
    public static class BootBranchExtensions
    {
        public static void NestBranchesRecursively(this IBootBranch root)
        {
            root.NestBranches();
            foreach (IBootBranch child in root.Children)
            {
                child.NestBranchesRecursively();
            }
        }

        public static void NestBranches(this IBootBranch root)
        {
            if (root.Children.Count == 0)
            {
                return;
            }
            
            var lookup = new Dictionary<Type, IBootBranch>();

            foreach (IBootBranch child in root.Children)
            {
                lookup[child.Boot.Type] = child;
            }
         
            foreach (IBootBranch child in lookup.Values.ToList())
            {
                if (child.Boot.TryGetControl(out IBootOverride overrideCtrl) 
                    && lookup.ContainsKey(overrideCtrl.Type))
                {
                    root.Children.Remove(lookup[overrideCtrl.Type]);
                    lookup[overrideCtrl.Type] = child;
                    lookup.Remove(child.Boot.Type);
                }
            }
            
            foreach (IBootBranch child in lookup.Values)
            {
                if (child.Boot.TryGetControl(out IBootInside parent) 
                    && parent.Type != root.Boot.Type)
                {
                    if (!lookup.ContainsKey(parent.Type))
                    {
                        throw new BootstrapException(
                            $"Unable to nest child {child.Boot.Type} in {parent.Type}" +
                            $" that is not presented in the {root.Boot.Type}");
                    }
                    
                    lookup[parent.Type].Children.Add(lookup[child.Boot.Type]);
                    root.Children.Remove(child);
                }
            }
        }
        
        /// <summary>
        /// Unchecked
        /// Before: [:i] insert before [before|end]
        /// After: [:i] insert after [after|end]
        /// </summary>
        public static void SortBranches(this IBootBranch root)
        {
            // After
            foreach (IBootBranch branch in root.Children.ToArray())
            {
                int index = root.Children.IndexOf(branch);
                int insertAfter;

                if (branch.Boot.TryGetControl(out IBootAfter after))
                {
                    // Find 'IBootAfter' branch index
                    for (insertAfter = index; insertAfter < root.Children.Count; insertAfter++)
                    {
                        if (root.Children[insertAfter].Boot.Type == after.Type)
                        {
                            insertAfter++;
                            break;
                        }
                    }
                }
                else
                {
                    // Last index
                    insertAfter = root.Children.Count - 1;
                }

                // Move all elements before the branch index including the branch
                // after the index of 'IBootAfter' branch
                for (int i = 0; i < index; i++)
                {
                    root.Children.Insert(insertAfter, root.Children[0]);
                    root.Children.RemoveAt(0);
                }
            }
        }

        public static void ProcessTree(this IBootBranch root, Action<IBootBranch> processor)
        {
            processor(root);

            foreach (IBootBranch child in root.Children)
            {
                child.ProcessTree(processor);
            }
        }
    }
}