using Unity.Entities;

namespace PeacefulHills.ECS.World
{
    public static class WorldExtensions
    {
        public static TExtension GetExtension<TExtension>(this Unity.Entities.World world)
            where TExtension : IWorldExtension
        {
            return WorldExtension<TExtension>.Get(world.SequenceNumber);
        }

        public static bool HasExtension<TExtension>(this Unity.Entities.World world)
            where TExtension : IWorldExtension
        {
            return WorldExtension<TExtension>.Exist(world.SequenceNumber);
        }

        public static void SetExtension<TExtension>(this Unity.Entities.World world, TExtension extension)
            where TExtension : IWorldExtension
        {
            WorldExtension<TExtension>.Set(world.SequenceNumber, extension);
        }

        public static void RemoveExtension<TExtension>(this Unity.Entities.World world)
            where TExtension : IWorldExtension
        {
            WorldExtension<TExtension>.Remove(world.SequenceNumber);
        }

        public static void RequestExtension<TExtension>(this Unity.Entities.World world,
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
                WorldExtension<TExtension>.Request(worldSequence,
                                                   extension => { system.CreateSingleton<TExtension>(); });
            }
            else if (!system.HasSingleton<ExtensionSingleton<TExtension>>())
            {
                system.CreateSingleton<TExtension>();
            }

            system.RequireSingletonForUpdate<ExtensionSingleton<TExtension>>();
        }

        private static void CreateSingleton<TExtension>(this ComponentSystemBase system)
            where TExtension : IWorldExtension
        {
            system.EntityManager.CreateEntity(typeof(ExtensionSingleton<TExtension>));
        }
    }
}