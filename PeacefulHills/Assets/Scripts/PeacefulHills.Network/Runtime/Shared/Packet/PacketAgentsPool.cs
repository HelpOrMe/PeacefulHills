using Unity.Entities;

namespace PeacefulHills.Network.Packet
{
    public struct PacketAgentsPool : IBufferElementData
    {
        /// <summary>
        /// Link to the packet agent entity with:
        ///  1. <see cref="PacketReceiveBuffer"/>
        ///  2. <see cref="PacketSendBuffer"/>
        ///  3. <see cref="ConnectionLink"/>
        ///  4. Components provided by <see cref="PacketType"/>
        /// </summary>
        public Entity Entity;
    }
}