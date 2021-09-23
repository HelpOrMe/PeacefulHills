using System.Collections.Generic;
using PeacefulHills.Extensions;
using PeacefulHills.Network.Packet;
using Unity.Entities;

[assembly: RegisterGenericComponentType(typeof(ExtensionSingleton<IPacketRegistry>))]

namespace PeacefulHills.Network.Packet
{
    /// <summary>
    /// Provides the ability to register a new package type.
    /// </summary>
    public interface IPacketRegistry : IWorldExtension
    {
        IEnumerable<PacketType> Packets { get; }

        /// <summary>
        /// Register a new packet type.
        /// When a connection is established, an Entity is created with DriverConnection and PacketAgentPool
        /// with links to packet agent entities. The packet agent entity will have the receive/send buffers,
        /// the link to the connection entity and the components you passed. All received data with the packet
        /// id in header will be copied to the corresponding packet agent receive buffer. 
        /// </summary>
        /// <seealso cref="PacketAgentsPool"/>
        /// <param name="components">Additional components with which the Packet Agent Entity will be created.
        /// I recommend to pass only tags (zero-sized components) for better chunks utilization.
        /// </param>
        PacketType RegisterNewPacketType(ComponentTypes components = default);

        bool ContainsPacketType(byte id);
        
        PacketType GetPacketType(byte id);
    }
}