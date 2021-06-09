using PeacefulHills.ECS.World;
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
        }

        protected virtual INetwork InitializeNetwork()
        {
            var driver = NetworkDriver.Create();

            var network = new Network {Driver = driver, DriverConcurrent = driver.ToConcurrent()};

            NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
            endpoint.Port = 9000;

            if (driver.Bind(endpoint) != 0)
            {
                throw new NetworkSimulationException("Unable to bind port " + endpoint.Port);
            }

            driver.Listen();
            return network;
        }

        protected override void OnUpdate()
        {
        }
    }
}