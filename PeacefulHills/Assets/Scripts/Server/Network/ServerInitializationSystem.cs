using PeacefulHills.ECS;
using PeacefulHills.Network.Messages;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    public class ServerInitializationSystem : SystemBase
    {
        protected override void OnCreate()
        {
            World.SetExtension(InitializeNetwork());
            World.SetExtension(InitializeMessagesRegistry());
        }
        
        protected virtual INetwork InitializeNetwork()
        {
            var network = new Network
            {
                Driver = NetworkDriver.Create()
            };

            NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
            endpoint.Port = 9000;
            
            if (network.Driver.Bind(endpoint) != 0)
            {
                throw new NetworkSimulationException("Unable to bind port " + endpoint.Port);
            }

            network.Driver.Listen();
            return network;
        }

        protected virtual IMessagesRegistry InitializeMessagesRegistry() => new MessagesRegistry();

        protected override void OnUpdate() { }
    }
}
