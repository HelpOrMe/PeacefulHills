using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public static partial class NetworkMessages
    {
        public static Entity Receive<TMessage>(MessageDeserializerContext context, TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = Create(context.CommandBuffer, context.SortKey, context.Connection, message);
            context.CommandBuffer.AddComponent<MessageReceiveRequest>(context.SortKey, entity);
            return entity;
        }
    }
}