using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class NetworkSendMessagesGroup : ComponentSystemGroup
    {
        
    }
}