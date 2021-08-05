using PeacefulHills.Extensions.World;
using PeacefulHills.Network.Profiling;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    // todo: del
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    public class EstablishClientConnection : SystemBase
    {
        protected override void OnCreate()
        {
            World.RequestExtension<INetwork>(ConnectToServer);
        }

        private void ConnectToServer(INetwork network)
        {
            NetworkEndPoint endpoint = NetworkEndPoint.LoopbackIpv4;
            endpoint.Port = 9000;
            
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);

            if (WorldsInitializationSettings.Load().hostWorld)
            {
                ConnectionBuilder.CreateHostConnection(commandBuffer, default);
            }
            else
            {
                NetworkConnection connection = network.Driver.Connect(endpoint);
                ConnectionBuilder.CreateConnection(commandBuffer, connection);
            }
            commandBuffer.Playback(EntityManager);
        }

        protected override void OnUpdate()
        {
        }
    }
}