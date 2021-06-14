using PeacefulHills.ECS.World;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    public class SetupDriverEndpointSystem : SystemBase
    {
        protected override void OnCreate()
        {
            World.RequestExtension<INetwork>(SetupDriverEndpoint);
        }

        protected void SetupDriverEndpoint(INetwork network)
        {
            NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
            endpoint.Port = 9000;

            if (network.Driver.Bind(endpoint) != 0)
            {
                throw new NetworkSimulationException("Unable to bind port " + endpoint.Port);
            }

            network.Driver.Listen();
        }

        protected override void OnUpdate()
        {
        }
    }
}