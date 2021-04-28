using PeacefulHills.Network.Messages;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Examples
{
    public struct BulletMessage : IMessage
    {
        public float2 Position;
        public float2 Rotation;
        public float Speed;
    }

    public class ExampleSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entity entity = EntityManager.CreateEntity(typeof(BulletMessage), typeof(SendMessageRequest));
            EntityManager.SetComponentData(entity, new BulletMessage
            {
                Position = new float2(15f, 3.2f),
                Rotation = new float2(90f, 90f),
                Speed = 2.3f,
            });
        }
    }
}