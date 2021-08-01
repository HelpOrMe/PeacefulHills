using PeacefulHills.ECS.World;
using PeacefulHills.Network;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    public class EstablishClientConnection : SystemBase
    {
        private NetworkConnection _connection;

        protected override void OnCreate()
        {
            World.RequestExtension<INetwork>(ConnectToServer);
        }

        private void ConnectToServer(INetwork network)
        {
            NetworkEndPoint endpoint = NetworkEndPoint.LoopbackIpv4;
            endpoint.Port = 9000;
            
            _connection = network.Driver.Connect(endpoint);
            
            var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
            ConnectionBuilder.CreateConnection(commandBuffer, _connection);
            commandBuffer.Playback(EntityManager);
        }

        protected override void OnUpdate()
        {
        }
    }
}