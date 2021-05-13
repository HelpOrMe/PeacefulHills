using Unity.Entities;

namespace PeacefulHills.ECS
{
    public static class WorldExtensions
    {
        public static TExtension GetExtension<TExtension>(this World world) 
            where TExtension : IWorldExtension 
            => WorldExtension<TExtension>.Get(world.SequenceNumber);

        public static bool HasExtension<TExtension>(this World world)
            where TExtension : IWorldExtension 
            => WorldExtension<TExtension>.Exist(world.SequenceNumber);

        public static void RequestExtension<TExtension>(this World world,
            WorldExtension<TExtension>.RequestAction request)
            where TExtension : IWorldExtension 
            => WorldExtension<TExtension>.Request(world.SequenceNumber, request);
        
        public static void SetExtension<TExtension>(this World world, TExtension extension) 
            where TExtension : IWorldExtension 
            => WorldExtension<TExtension>.Set(world.SequenceNumber, extension);
        
        public static void RequireExtension<TExtension>(this ComponentSystemBase system)
            where TExtension : IWorldExtension
        {
            ulong worldSequence = system.World.SequenceNumber;
            if (!WorldExtension<TExtension>.Exist(worldSequence))
            {
                WorldExtension<TExtension>.Request(worldSequence, extension =>
                {
                    system.SetSingleton(new ExtensionSingleton<TExtension>());
                });
            }
            else if (!system.HasSingleton<ExtensionSingleton<TExtension>>())
            {
                system.SetSingleton(new ExtensionSingleton<TExtension>());
            }
            
            system.RequireSingletonForUpdate<ExtensionSingleton<TExtension>>();
        }
    }
}