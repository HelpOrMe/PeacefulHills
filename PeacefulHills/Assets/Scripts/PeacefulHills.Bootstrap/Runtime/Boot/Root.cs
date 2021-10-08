using System;
using System.Collections.Generic;
using PeacefulHills.Bootstrap.Index;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PeacefulHills.Bootstrap
{
    public static class Root
    {
        public static readonly Boot Boot = new RootBoot();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Call()
        {
            #if !UNITY_EDITOR
            Setup();
            #endif            
            Boot.Call();
        }
        
        #if UNITY_EDITOR
        [InitializeOnLoadMethod]
        #endif
        private static void Setup()
        {
            IEnumerable<Type> bootTypes = TypeIndexer.IndexImplementation<Boot>();

            foreach (Type bootType in bootTypes)
            {
                Boot.Children.Add((Boot)Activator.CreateInstance(bootType));
            }
            
            Boot.Setup();
        }
    }
}