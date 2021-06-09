using Unity.Entities;

namespace PeacefulHills.Network.Connection
{
    /// <summary>
    ///     NetworkStreamConnection entity with this tag will be disconnected
    /// </summary>
    public struct InterruptNetworkConnection : IComponentData
    {
    }

    /// <summary>
    ///     Will be added after the client disconnects or after adding the InterruptConnection tag.
    ///     After one tick an entity with this tag will be destroyed!
    /// </summary>
    public struct InterruptedNetworkConnection : IComponentData
    {
    }
}