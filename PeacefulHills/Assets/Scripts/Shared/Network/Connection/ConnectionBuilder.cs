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
                    
            commandBuffer.SetComponent(entity, new ConnectionWrapper {Connection = connection});
            commandBuffer.SetBuffer<MessagesSendBuffer>(entity);
        }
    }
}