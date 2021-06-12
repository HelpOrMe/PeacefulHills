using PeacefulHills.Network.Messages;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    public struct ConnectionBuilder
    {
        public static void CreateConnection(EntityCommandBuffer commandBuffer, NetworkConnection connection)
        {
            Entity entity = commandBuffer.CreateEntity();
                    
            commandBuffer.AddComponent(entity, new ConnectionWrapper {Connection = connection});
            commandBuffer.AddBuffer<MessagesSendBuffer>(entity);
        }
    }
}