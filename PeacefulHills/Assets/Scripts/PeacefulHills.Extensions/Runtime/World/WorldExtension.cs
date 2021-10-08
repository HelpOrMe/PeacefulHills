using System.Collections.Generic;

namespace PeacefulHills.Extensions
{
    public static class WorldExtension<TExtension> where TExtension : IWorldExtension
    {
        public delegate void RequestAction(TExtension extension);

        private static readonly Dictionary<ulong, WorldExtensionInfo> Lookup =
            new Dictionary<ulong, WorldExtensionInfo>();

        public static bool Exist(ulong worldSequence)
        {
            return Lookup.ContainsKey(worldSequence) && Lookup[worldSequence].Instance != null;
        }

        public static TExtension Get(ulong worldSequenceNumber)
        {
            //todo: throw on not found
            return Lookup[worldSequenceNumber].Instance;
        }

        public static void Set(ulong worldSequence, TExtension extension)
        {
            if (Lookup.TryGetValue(worldSequence, out WorldExtensionInfo info))
            {
                info.Instance = extension;
                info.RequestAction?.Invoke(extension);
                return;
            }

            Lookup[worldSequence] = new WorldExtensionInfo {Instance = extension};
        }

        public static void Remove(ulong worldSequenceNumber)
        {
            Lookup.Remove(worldSequenceNumber);
        }

        public static void Request(ulong worldSequence, RequestAction request)
        {
            if (Lookup.TryGetValue(worldSequence, out WorldExtensionInfo extension))
            {
                if (extension.Instance != null)
                {
                    request(extension.Instance);
                    return;
                }

                extension.RequestAction += request;
                return;
            }

            Lookup[worldSequence] = new WorldExtensionInfo {RequestAction = request};
        }

        public class WorldExtensionInfo
        {
            public TExtension Instance;
            public RequestAction RequestAction;
        }
    }
}