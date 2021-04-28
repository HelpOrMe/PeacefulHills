using Unity.Entities;

namespace PeacefulHills.Network.Messages
{
    public class ReceiveMessageSystemBase<TMessage> : SystemBase 
        where TMessage : struct, IMessage
    {
        protected override void OnCreate()
        {
            
        }

        protected override void OnUpdate()
        {
            
        }

        public virtual void ReceiveMessage(TMessage message)
        {
            
        }
    }
}