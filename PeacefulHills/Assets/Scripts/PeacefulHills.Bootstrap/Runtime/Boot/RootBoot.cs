using System;
using System.Collections.Generic;

namespace PeacefulHills.Bootstrap
{
    public class RootBoot : Boot
    {
        public override void Setup()
        {
            SetupChildren();
            NestChildren();
            
            Instructions.Add(new CallChildrenInstruction());
        }

        private void SetupChildren()
        {
            Children.Propagate(b => b.Setup());
        }
        
        private void NestChildren()
        {
            var lookup = new Dictionary<Type, Boot>();
            
            foreach (Boot child in Children)
            {
                lookup[child.GetType()] = child;
            }
            
            foreach (Boot child in lookup.Values)
            {
                if (child.Instructions.TryFind(out BootInside bootInside) 
                    && lookup.ContainsKey(bootInside.Type))
                {
                    lookup[bootInside.Type].Children.Add(child); 
                    Children.Remove(child);
                }
            }
        }
    }
}