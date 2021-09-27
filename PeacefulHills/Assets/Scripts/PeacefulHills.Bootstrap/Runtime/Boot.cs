using System;
using PeacefulHills.Bootstrap.Index;
using PeacefulHills.Bootstrap.Tree;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PeacefulHills.Bootstrap
{
    [BootNamed("Root")]
    public class Boot : IBoot
    {
        public static IBootBranch Root { get; private set; }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Run()
        {
            #if !UNITY_EDITOR
            Build();
            #endif
            Root.PropagateInvoke();
        }
        
        #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        #endif
        public static void Build()
        {
            Root = BootBranch.Build(new BootInfo(typeof(Boot)));

            foreach (Type bootType in TypeIndexer.IndexInherited<IBoot>())
            {
                Root.Children.Add(BootBranch.Build(new BootInfo(bootType)));
            }
            
            Root.NestBranchesRecursively();
        }
    }
}