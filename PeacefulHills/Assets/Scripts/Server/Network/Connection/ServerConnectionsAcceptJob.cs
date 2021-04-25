using Unity.Entities;
using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    [UpdateInGroup(typeof(ServerNetworkUpdateGroup))]
    public struct ServerConnectionsAcceptJob : IJob
    {
        public NetworkDriver Driver;
        public EntityCommandBuffer CommandBuffer;

        public void Execute()
        {
            NetworkConnection connection;
            while ((connection = Driver.Accept()) != default)
            {
                if (connection.PopEvent(Driver, out DataStreamReader _) != NetworkEvent.Type.Empty)
                {
                    connection.Disconnect(Driver);
                    continue;
                }

                Entity entity = CommandBuffer.CreateEntity();
                CommandBuffer.SetComponent(entity, new NetworkStreamConnection { Connection = connection });
            }
        }
    }
}