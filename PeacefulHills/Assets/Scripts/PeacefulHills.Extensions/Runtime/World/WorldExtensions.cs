using Unity.Entities;

namespace PeacefulHills.Extensions
{
    public static class WorldExtensions
    {
        public static TExtension GetExtension<TExtension>(this World world)
            where TExtension : IWorldExtension
        {
            return WorldExtension<TExtension>.Get(world.SequenceNumber);
        }

        public static bool HasExtension<TExtension>(this World world)
            where TExtension : IWorldExtension
        {
            return WorldExtension<TExtension>.Exist(world.SequenceNumber);
        }

        public static void SetExtension<TExtension>(this World world, TExtension extension)
            where TExtension : IWorldExtension
        {
            WorldExtension<TExtension>.Set(world.SequenceNumber, extension);
        }

        public static void RemoveExtension<TExtension>(this World world)
            where TExtension : IWorldExtension
        {
            WorldExtension<TExtension>.Remove(world.SequenceNumber);
        }

        public static void RequestExtension<TExtension>(this World world, 
            WorldExtension<TExtension>.RequestAction request)
            where TExtension : IWorldExtension
        {
            WorldExtension<TExtension>.Request(world.SequenceNumber, request);
        }

        public static void RequireExtension<TExtension>(this ComponentSystemBase system)
            where TExtension : IWorldExtension
        {
            ulong worldSequence = system.World.SequenceNumber;
            if (!WorldExtension<TExtension>.Exist(worldSequence))
            {
                WorldExtension<TExtension>.Request(worldSequence, extension =>
                {
                    system.CreateExtensionSingleton<TExtension>();
                });
            }
            else if (!system.HasExtensionSingleton<TExtension>())
            {
                system.CreateExtensionSingleton<TExtension>();
            }

            system.RequireSingletonForUpdate<ExtensionSingleton<TExtension>>();
        }

        private static void CreateExtensionSingleton<TExtension>(this ComponentSystemBase system)
            where TExtension : IWorldExtension
        {
            system.EntityManager.CreateEntity(typeof(ExtensionSingleton<TExtension>));
        }

        private static bool HasExtensionSingleton<TExtension>(this ComponentSystemBase system)
            where TExtension : IWorldExtension
        {
            return system.HasSingleton<ExtensionSingleton<TExtension>>();
        }
    }
}