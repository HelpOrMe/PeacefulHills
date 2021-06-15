using PeacefulHills.Network.Messages;
using PeacefulHills.Network.Receive;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    // TODO: Temporary solution
    public static class ConnectionBuilder
    {
        public static void CreateConnection(EntityCommandBuffer commandBuffer, NetworkConnection connection)
        {
            Entity connectionEntity = commandBuffer.CreateEntity();
            
            commandBuffer.AddComponent(connectionEntity, new ConnectionWrapper {Value = connection});
            
            commandBuffer.AddBuffer<MessagesSendBuffer>(connectionEntity).Add(new MessagesSendBuffer 
            {
                Value = (byte)NetworkPackageType.Message
            });
            commandBuffer.AddBuffer<NetworkReceiveBufferPool>(connectionEntity).Add(new NetworkReceiveBufferPool
            {
                Entity = CreateReceiveBufferEntity(commandBuffer)
            });
        }

        private static Entity CreateReceiveBufferEntity(EntityCommandBuffer commandBuffer)
        {
            Entity entity = commandBuffer.CreateEntity();
            commandBuffer.AddBuffer<NetworkReceiveBuffer>(entity);
            return entity;
        }
    }
}