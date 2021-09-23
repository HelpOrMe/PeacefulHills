using PeacefulHills.Network.Packet;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    /// <summary>
    /// Tag that marks entity as message's packet agent entity.
    /// <seealso cref="PacketAgentsPool"/>
    /// </summary>
    public struct MessagePacketAgent : IComponentData
    {
    }
}