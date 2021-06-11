using Unity.Entities;

namespace PeacefulHills.Network.Connection
{
    /// <summary>
    ///     A connection entity with this tag will be disconnected and destroyed
    /// </summary>
    public struct ConnectionInterrupt : IComponentData
    {
    }
}