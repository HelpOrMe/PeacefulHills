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
            // 
            // var buffer = Network.GetMessageBuffer<BulletMessage>(Connection);
            // buffer.Add
            
            // Entity entity = NetworkHelper.Send(this, connection, new BulletMessage {})
            // Entity entity = EntityManager.CreateEntity(typeof(BulletMessage), typeof(SendMessageRequest), typeof(TargetConnection));
            // EntityManager.SetComponentData(entity, new BulletMessage
            // {
                // Position = new float2(15f, 3.2f),
                // Rotation = new float2(90f, 90f),
                // Speed = 2.3f,
            // });
        }
    }
}