using Unity.Entities;

namespace PeacefulHills.ECS
{
    public static class WorldExtensions
    {
        public static TExtension GetExtension<TExtension>(this World world) 
            where TExtension : IWorldExtension 
            => WorldExtension<TExtension>.Get(world);

        public static bool HasExtension<TExtension>(this World world)
            where TExtension : IWorldExtension 
            => WorldExtension<TExtension>.Exist(world);

        public static void RequestExtension<TExtension>(this World world,
            WorldExtension<TExtension>.RequestAction request)
            where TExtension : IWorldExtension 
            => WorldExtension<TExtension>.Request(world, request);
        
        public static void SetExtension<TExtension>(this World world, TExtension extension) 
            where TExtension : IWorldExtension 
            => WorldExtension<TExtension>.Set(world, extension);
        
        public static void RequireExtension<TExtension>(this ComponentSystemBase system)
            where TExtension : IWorldExtension
        {
            if (!WorldExtension<TExtension>.Exist(system.World))
            {
                WorldExtension<TExtension>.Request(system.World, extension =>
                {
                    system.SetSingleton(new ExtensionSingleton<TExtension>());
                });
            }
            system.RequireSingletonForUpdate<ExtensionSingleton<TExtension>>();
        }
    }
}