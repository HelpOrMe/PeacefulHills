using PeacefulHills.Extensions;
using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public static partial class NetworkMessages
    {
        public static Entity Broadcast<TMessage>(EntityManager entityManager, TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = entityManager.CreateEntity(typeof(MessageSendRequest), typeof(MessageTarget), typeof(TMessage));
            entityManager.SetComponentData(entity, message);
            return entity;
        }

        public static Entity Broadcast<TMessage>(EntityCommandBuffer commandBuffer, TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = commandBuffer.CreateEntity();
            commandBuffer.AddComponent<MessageSendRequest>(entity);
            commandBuffer.AddComponent<MessageTarget>(entity);
            commandBuffer.AddComponent<TMessage>(entity);
            commandBuffer.SetComponent(entity, message);
            return entity;
        }

        public static Entity Broadcast<TMessage>(EntityCommandBuffer.ParallelWriter commandBuffer, int sortKey,
                                                 TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = commandBuffer.CreateEntity(sortKey);
            commandBuffer.AddComponent<MessageSendRequest>(sortKey, entity);
            commandBuffer.AddComponent<MessageTarget>(sortKey, entity);
            commandBuffer.AddComponent<TMessage>(sortKey, entity);
            commandBuffer.SetComponent(sortKey, entity, message);
            return entity;
        }

        public static Entity Send<TMessage>(EntityManager entityManager, Entity connection, TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = Create(entityManager, connection, message);
            entityManager.AddComponent<MessageSendRequest>(entity);
            return entity;
        }

        public static Entity Send<TMessage>(EntityCommandBuffer commandBuffer, Entity connection, TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = Create(commandBuffer, connection, message);
            commandBuffer.AddComponent<MessageSendRequest>(entity);
            return entity;
        }

        public static Entity Send<TMessage>(EntityCommandBuffer.ParallelWriter commandBuffer, int sortKey,
                                            Entity connection, TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = Create(commandBuffer, sortKey, connection, message);
            commandBuffer.AddComponent<MessageSendRequest>(sortKey, entity);
            return entity;
        }

        public static Entity Receive<TMessage>(EntityManager entityManager, Entity connection, TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = Create(entityManager, connection, message);
            entityManager.AddComponent<MessageReceiveRequest>(entity);
            return entity;
        }

        public static Entity Receive<TMessage>(EntityCommandBuffer commandBuffer, Entity connection, TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = Create(commandBuffer, connection, message);
            commandBuffer.AddComponent<MessageReceiveRequest>(entity);
            return entity;
        }

        public static Entity Receive<TMessage>(EntityCommandBuffer.ParallelWriter commandBuffer, int sortKey,
                                               Entity connection, TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = Create(commandBuffer, sortKey, connection, message);
            commandBuffer.AddComponent<MessageReceiveRequest>(sortKey, entity);
            return entity;
        }

        private static Entity Create<TMessage>(EntityManager entityManager, Entity connection, TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = entityManager.CreateEntity(typeof(MessageTarget), typeof(TMessage));
            entityManager.SetComponentData(entity, new MessageTarget {Connection = connection});
            entityManager.SetComponentData(entity, message);
            return entity;
        }

        private static Entity Create<TMessage>(EntityCommandBuffer commandBuffer, Entity connection, TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = commandBuffer.CreateEntity();
            commandBuffer.AddComponent<MessageTarget>(entity);
            commandBuffer.AddComponent<TMessage>(entity);
            commandBuffer.SetComponent(entity, new MessageTarget {Connection = connection});
            commandBuffer.SetComponent(entity, message);
            return entity;
        }

        private static Entity Create<TMessage>(EntityCommandBuffer.ParallelWriter commandBuffer, int sortKey,
                                               Entity connection, TMessage message)
            where TMessage : unmanaged, IMessage, IComponentData
        {
            Entity entity = commandBuffer.CreateEntity(sortKey);
            commandBuffer.AddComponent<MessageTarget>(sortKey, entity);
            commandBuffer.AddComponent<TMessage>(sortKey, entity);
            commandBuffer.SetComponent(sortKey, entity, new MessageTarget {Connection = connection});
            commandBuffer.SetComponent(sortKey, entity, message);
            return entity;
        }

        public static MessagesScheduler<TMessage, TMessageSerializer> Scheduler<TMessage, TMessageSerializer>(
            World world)
            where TMessage : unmanaged, IMessage
            where TMessageSerializer : unmanaged, IMessageSerializer<TMessage>
        {
            ushort messageId = world.GetExtension<IMessageRegistry>().GetId<TMessage>();
            return new MessagesScheduler<TMessage, TMessageSerializer>(messageId);
        }
    }
}