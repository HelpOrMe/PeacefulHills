using PeacefulHills.Extensions;
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

            if (NetworkWorldsInitSettings.Current.SplitWorlds)
            {
                NetworkConnection connection = network.Driver.Connect(endpoint);
                ConnectionBuilder.CreateConnection(commandBuffer, connection);
            }
            else
            {
                ConnectionBuilder.CreateHostConnection(commandBuffer, default);
            }
            commandBuffer.Playback(EntityManager);
        }

        protected override void OnUpdate()
        {
        }
    }
}