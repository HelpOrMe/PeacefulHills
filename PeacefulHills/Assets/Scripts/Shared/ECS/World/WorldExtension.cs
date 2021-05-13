using System.Collections.Generic;

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

        public static bool Exist(ulong worldSequence) 
            => Lookup.ContainsKey(worldSequence) && Lookup[worldSequence].Instance != null;

        public static TExtension Get(ulong worldSequenceNumber) 
            => Lookup[worldSequenceNumber].Instance;

        public static void Set(ulong worldSequence, TExtension extension)
        {
            if (Lookup.TryGetValue(worldSequence, out WorldExtensionInfo info))
            {
                info.Instance = extension;
                info.Request?.Invoke(extension);
                return;
            }
            
            Lookup[worldSequence] = new WorldExtensionInfo { Instance = extension };
        }
        
        public static void Remove(ulong worldSequenceNumber) 
            => Lookup.Remove(worldSequenceNumber);
        
        public static void Request(ulong worldSequence, RequestAction request)
        {
            if (Lookup.TryGetValue(worldSequence, out WorldExtensionInfo extension))
            {
                if (extension.Instance != null)
                {
                    request(extension.Instance);
                    return;
                }
                extension.Request += request;
                return;
            }

            Lookup[worldSequence] = new WorldExtensionInfo { Request = request };
        }
    }
}