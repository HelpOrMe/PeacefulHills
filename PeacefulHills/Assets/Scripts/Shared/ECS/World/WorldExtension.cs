using System;
using System.Collections.Generic;
using Unity.Entities;

namespace PeacefulHills.ECS
{
    public static class WorldExtension<TExtension> where TExtension : IWorldExtension
    {
        public class WorldExtensionInfo
        {
            public TExtension Instance;
            public readonly List<RequestAction> Requests = new List<RequestAction>();
        }
        
        public delegate void RequestAction(TExtension extension);
        
        private static readonly Dictionary<ulong, WorldExtensionInfo> Lookup = 
            new Dictionary<ulong, WorldExtensionInfo>();
        
        public static TExtension Get(World world) 
            => Lookup[world.SequenceNumber].Instance;

        public static bool Exist(World world) 
            => Lookup.ContainsKey(world.SequenceNumber);

        public static void Request(World world, RequestAction request)
        {
            ulong sequence = world.SequenceNumber;
            if (Lookup.TryGetValue(sequence, out WorldExtensionInfo info))
            {
                if (info.Instance != null)
                {
                    request(info.Instance);
                    return;
                }
                info.Requests.Add(request);
                return;
            }
            
            info = new WorldExtensionInfo();
            info.Requests.Add(request);
            Lookup[world.SequenceNumber] = info;
        }
        
        public static void Add(World world, TExtension extension)
        {
            ulong sequence = world.SequenceNumber;
            if (Lookup.TryGetValue(sequence, out WorldExtensionInfo info))
            {
                info.Instance = extension;
                foreach (RequestAction request in info.Requests)
                {
                    request(extension);
                }
                info.Requests.Clear();
                return;
            }
            
            Lookup[world.SequenceNumber] = new WorldExtensionInfo { Instance = extension };
        }
    }
}