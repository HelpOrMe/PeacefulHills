using Unity.Entities;

namespace PeacefulHills.Network.Packet
{
    /// <summary>
    /// Info about packet that was registered with <see cref="IPacketRegistry"/>
    /// </summary>
    public readonly struct PacketType
    {
        public readonly byte Id;
        public readonly ComponentTypes AgentComponents;

        public PacketType(byte id, ComponentTypes agentComponents)
        {
            Id = id;
            AgentComponents = agentComponents;
        }

        public override bool Equals(object obj)
        {
            return obj is PacketType other && Equals(other);
        }

        public bool Equals(PacketType other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}