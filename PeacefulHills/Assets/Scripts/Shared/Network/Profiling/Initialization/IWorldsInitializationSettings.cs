#if UNITY_EDITOR

namespace PeacefulHills.Network.Profiling
{
    public interface IWorldsInitializationSettings
    {
        bool HostWorld { get; }
        int ClientCount { get; }
    }
}

#endif