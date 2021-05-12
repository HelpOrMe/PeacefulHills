using System.Collections.Generic;
using Unity.Entities;

namespace PeacefulHills.ECS
{
    public static class WorldExtension<TExtension> where TExtension : IWorldExtension
    {
        public class WorldExtensionInfo
        {
            public TExtension Instance;
            public RequestAction Request;
        }
        
        public delegate void RequestAction(TExtension extension);
        
        private static readonly Dictionary<ulong, WorldExtensionInfo> Lookup = 
            new Dictionary<ulong, WorldExtensionInfo>();

        public static bool Exist(World world)
        {
            ulong sequence = world.SequenceNumber;
            return Lookup.ContainsKey(sequence) || Lookup[sequence].Instance != null;
        }
        
        public static TExtension Get(World world) 
            => Lookup[world.SequenceNumber].Instance;

        public static void Set(World world, TExtension extension)
        {
            ulong sequence = world.SequenceNumber;
            if (Lookup.TryGetValue(sequence, out WorldExtensionInfo info))
            {
                info.Instance = extension;
                info.Request?.Invoke(extension);
                return;
            }
            
            Lookup[sequence] = new WorldExtensionInfo { Instance = extension };
        }
        
        public static void Request(World world, RequestAction request)
        {
            ulong sequence = world.SequenceNumber;
            if (Lookup.TryGetValue(sequence, out WorldExtensionInfo extension))
            {
                if (extension.Instance != null)
                {
                    request(extension.Instance);
                    return;
                }
                extension.Request += request;
                return;
            }

            Lookup[sequence] = new WorldExtensionInfo { Request = request };
        }
    }
}