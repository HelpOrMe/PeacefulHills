#if UNITY_EDITOR

using PeacefulHills.ECS.World;
using PeacefulHills.Network.Profiling;
using Unity.Entities;

[assembly: RegisterGenericComponentType(typeof(ExtensionSingleton<IProfilingSettings>))]

namespace PeacefulHills.Network.Profiling
{
    public interface IProfilingSettings : IWorldExtension
    {
        bool SeparateWorlds { get; }
        int ClientCount { get; }
    }
}

#endif