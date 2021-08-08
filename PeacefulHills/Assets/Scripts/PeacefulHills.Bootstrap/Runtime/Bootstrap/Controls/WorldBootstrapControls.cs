using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.Collections;
using UnityEngine;

namespace PeacefulHills.Bootstrap.Controls
{
    public class WorldBootstrapControls : IWorldBootstrapControls
    {
        public WorldBootstrapInfo Determine(WorldBootstrapInfo bootstrapInfo)
        {
            var overloadBootstrapsInfo = new List<WorldBootstrapInfo>();
            
            foreach (WorldBootstrapInfo overloadBootstrapInfo in bootstrapInfo.OverloadBootstraps)
            {
                if (overloadBootstrapInfo.Type.TryGetAttribute(out BootstrapConditionAttribute condition) 
                    && !condition.Check(overloadBootstrapInfo))
                {
                    continue;
                }
                overloadBootstrapsInfo.Add(Determine(overloadBootstrapInfo));
            }
            
            if (overloadBootstrapsInfo.Count == 0)
            {
                return bootstrapInfo;
            }
            
            if (bootstrapInfo.Type.TryGetAttribute(out BootstrapResolverAttribute resolver))
            {
                return resolver.Resolve(overloadBootstrapsInfo);
            }
            
            foreach (WorldBootstrapInfo overloadBootstrapInfo in overloadBootstrapsInfo)
            {
                if (bootstrapInfo.Type.HasAttribute<BootstrapFallbackAttribute>())
                {
                    return overloadBootstrapInfo;
                }
            }

            if (overloadBootstrapsInfo.Count > 1)
            {
                throw new BootstrapControlsException("An ambiguous choice between bootstraps: " 
                                                     + string.Join(", ", overloadBootstrapsInfo.Select(b => b.Type.Name)));
            }

            return overloadBootstrapsInfo[0];
        }
    }
}