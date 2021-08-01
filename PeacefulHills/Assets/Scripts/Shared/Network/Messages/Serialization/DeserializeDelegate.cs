using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    public delegate void DeserializeAction(ref DataStreamReader reader, ref MessageDeserializerContext context);
}