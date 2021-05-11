﻿using Unity.Entities;

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
            => WorldExtension<TExtension>.Add(world, extension);
    }
}