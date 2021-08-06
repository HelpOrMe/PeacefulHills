using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PeacefulHills.Bootstrap
{
    public static class WorldsBootstrap 
    {
        private static readonly List<WorldBootstrapContext> Contexts;

        static WorldsBootstrap()
        {
            var builder = new WorldBootstrapBuilder();
            Contexts = builder.BuildBootstrapContexts();
        }
        
        public static void Initialize()
        {
            foreach (WorldBootstrapContext worldBootstrapContext in Contexts)
            {
                worldBootstrapContext.Bootstrap.Initialize();
            }
        }

        public static void Initialize(string pattern)
        {
            foreach (WorldBootstrapContext worldBootstrapContext in Contexts)
            {
                if (Regex.IsMatch(worldBootstrapContext.Info.Type.Name, pattern))
                {
                    worldBootstrapContext.Bootstrap.Initialize();
                }
            }
        }

        public static void Initialize<TWorldBootstrap>() where TWorldBootstrap : WorldBootstrapBase
        {
            foreach (WorldBootstrapContext worldBootstrapContext in Contexts)
            {
                if (worldBootstrapContext.Bootstrap is TWorldBootstrap)
                {
                    worldBootstrapContext.Bootstrap.Initialize();
                }
            }
        }
    }
}