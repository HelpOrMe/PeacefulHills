using System.Collections.Generic;
using Unity.Entities;

namespace PeacefulHills.Network.Packet
{
    public class PacketRegistry : IPacketRegistry
    {
        public IEnumerable<PacketType> Packets => _packetTypes;

        private readonly List<PacketType> _packetTypes = new List<PacketType>();
        
        public PacketType RegisterNewPacketType(ComponentTypes components = default)
        {
            if (_packetTypes.Count == byte.MaxValue)
            {
                throw new NetworkInitializationException("Unable to register more then 256 packets.");
            }
            
            var packet = new PacketType((byte)_packetTypes.Count, components);
            _packetTypes.Add(packet);
            return packet;
        }

        public bool ContainsPacketType(byte id)
        {
            return id < _packetTypes.Count;
        }

        public PacketType GetPacketType(byte id)
        {
            return _packetTypes[id];
        }
    }
}