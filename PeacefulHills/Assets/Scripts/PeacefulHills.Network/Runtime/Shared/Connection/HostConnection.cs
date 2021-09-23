#if !UNITY_SERVER || UNITY_EDITOR

using Unity.Entities;

namespace PeacefulHills.Network
{
    /// <summary>
    /// Mark connection as host, so a packet implementation can be optimized.
    /// </summary>
    public struct HostConnection : IComponentData
    {
    }
}

#endif