using PeacefulHills.Network.Messages;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    public static class ConnectionBuilder
    {
        public static void CreateConnection(EntityCommandBuffer commandBuffer, NetworkConnection connection)
        {
            Entity connectionEntity = commandBuffer.CreateEntity();
                    
            // TODO: Temporary solution
            
            commandBuffer.AddComponent(connectionEntity, new ConnectionWrapper {Value = connection});
            
            commandBuffer.AddBuffer<MessagesSendBuffer>(connectionEntity).Add(new MessagesSendBuffer 
            {
                Value = (byte)NetworkPackageType.Message
            });
        }
    }
}