using System;
using System.Collections.Generic;
using PeacefulHills.Bootstrap.Controls;

namespace PeacefulHills.Bootstrap
{
    public static class WorldBootstraps
    {
        private static readonly IWorldBootstrapsBuilder Builder = new WorldBootstrapBuilder();
        private static readonly IWorldBootstrapControls Controls = new WorldBootstrapControls();

        private static readonly Dictionary<Type, WorldBootstrapInfo> BootstrapsInfo;
        
        static WorldBootstraps()
        {
            BootstrapsInfo = Builder.Build();
        }

        public static void Initialize()
        {
            foreach (Type bootstrapsType in BootstrapsInfo.Keys)
            {
                InitializeInternal(bootstrapsType);
            }
        }

        public static void Initialize<TBootstrap>() where TBootstrap : WorldBootstrapBase
        {
            Initialize(typeof(TBootstrap));
        }
        
        public static void Initialize(Type bootstrapsType)
        {
            ThrowIfDoesNotExist(bootstrapsType);
            InitializeInternal(bootstrapsType);
        }

        public static void ForceInitialize<TBootstrap>() where TBootstrap : WorldBootstrapBase
        {
            ForceInitialize(typeof(TBootstrap));
        }
        
        public static void ForceInitialize(Type bootstrapsType)
        {
            ThrowIfDoesNotExist(bootstrapsType);
            WorldBootstrapInfo bootstrapInfo = BootstrapsInfo[bootstrapsType];
            InitializeInternal(bootstrapInfo);
        }

        private static void ThrowIfDoesNotExist(Type bootstrapsType)
        {
            if (!Exist(bootstrapsType))
            {
                throw new ArgumentException($"Unable to find bootstrap of type {bootstrapsType.Name}");
            }
        }

        public static bool Exist(Type bootstrapsType)
        {
            return BootstrapsInfo.ContainsKey(bootstrapsType);
        }

        private static void InitializeInternal(Type bootstrapsType)
        {
            WorldBootstrapInfo bootstrapInfo = BootstrapsInfo[bootstrapsType];
            if (bootstrapInfo.BaseBootstrap != null)
            {
                return;
            }
            
            InitializeInternal(Controls.Determine(bootstrapInfo));
        }
        
        private static void InitializeInternal(WorldBootstrapInfo bootstrapInfo)
        {
            var bootstrap = (WorldBootstrapBase) Activator.CreateInstance(bootstrapInfo.Type);
            bootstrap.Systems = bootstrapInfo.Systems;
            bootstrap.Initialize();
        }
     }
}