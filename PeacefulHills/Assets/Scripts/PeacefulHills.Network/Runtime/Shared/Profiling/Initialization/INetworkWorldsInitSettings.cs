#if UNITY_EDITOR

namespace PeacefulHills.Network.Profiling
{
    public interface INetworkWorldsInitSettings
    {
        bool SplitWorlds { get; }
        int ClientCount { get; }
    }
}

#endif