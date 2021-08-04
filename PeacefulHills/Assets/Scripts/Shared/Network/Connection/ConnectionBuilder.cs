using PeacefulHills.Network.Messages;
using PeacefulHills.Network.Receive;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    // TODO: Temporary solution
    public static class ConnectionBuilder
    {
        #if !UNITY_SERVER || UNITY_EDITOR

        public static Entity CreateHostConnection(EntityCommandBuffer commandBuffer, NetworkConnection connection)
        {
            Entity connectionEntity = CreateConnection(commandBuffer, connection);
            commandBuffer.AddComponent<HostConnection>(connectionEntity);
            return connectionEntity;
        }

        #endif

        public static Entity CreateConnection(EntityCommandBuffer commandBuffer, NetworkConnection connection)
        {
            Entity connectionEntity = commandBuffer.CreateEntity();

            commandBuffer.AddComponent(connectionEntity, new ConnectionWrapper {Value = connection});

            commandBuffer.AddBuffer<MessagesSendBuffer>(connectionEntity).Add(new MessagesSendBuffer
            {
                Value = (byte) NetworkPackageType.Message
            });
            commandBuffer.AddBuffer<NetworkReceiveBufferPool>(connectionEntity).Add(new NetworkReceiveBufferPool
            {
                Entity = CreateReceiveBufferEntity(commandBuffer)
            });

            return connectionEntity;
        }

        private static Entity CreateReceiveBufferEntity(EntityCommandBuffer commandBuffer)
        {
            Entity entity = commandBuffer.CreateEntity();
            commandBuffer.AddBuffer<NetworkReceiveBuffer>(entity);
            return entity;
        }
    }
}