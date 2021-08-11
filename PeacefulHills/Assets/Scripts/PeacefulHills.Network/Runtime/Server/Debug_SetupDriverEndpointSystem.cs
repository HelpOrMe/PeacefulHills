using PeacefulHills.Extensions;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    // todo: del
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    public class SetupDriverEndpointSystem : SystemBase
    {
        protected override void OnCreate()
        {
            World.RequestExtension<INetworkDriverInfo>(SetupDriverEndpoint);
        }

        protected void SetupDriverEndpoint(INetworkDriverInfo driver)    
        {
            NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
            endpoint.Port = 9000;

            if (driver.Current.Bind(endpoint) != 0)
            {
                throw new NetworkSimulationException("Unable to bind port " + endpoint.Port);
            }

            driver.Current.Listen();
        }

        protected override void OnUpdate()
        {
        }
    }
}