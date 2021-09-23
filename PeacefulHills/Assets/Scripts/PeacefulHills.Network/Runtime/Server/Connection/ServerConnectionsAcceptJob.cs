using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    [BurstCompile]
    public struct ServerConnectionsAcceptJob : IJob
    {
        public NetworkDriver Driver;
        public EntityCommandBuffer CommandBuffer;
        public ConnectionBuilder ConnectionBuilder;

        public void Execute()
        {
            NetworkConnection connection;
            while ((connection = Driver.Accept()) != default)
            {
                if (connection.PopEvent(Driver, out DataStreamReader _) == NetworkEvent.Type.Empty)
                {
                    ConnectionBuilder.CreateConnection(CommandBuffer, connection);
                }
            }
        }
    }
}