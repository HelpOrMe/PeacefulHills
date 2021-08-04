using Unity.Burst;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    [BurstCompile]
    public static class MessageSerializerStatic<TMessage, TMessageSerializer>
        where TMessage : unmanaged, IMessage
        where TMessageSerializer : unmanaged, IMessageSerializer<TMessage>
    {
        // ReSharper disable once StaticMemberInGenericType
        public static readonly FunctionPointer<DeserializeAction> DeserializeAction =
            BurstCompiler.CompileFunctionPointer<DeserializeAction>(Deserialize);

        [BurstCompile]
        private static void Deserialize(ref DataStreamReader reader, ref MessageDeserializerContext context)
        {
            default(TMessageSerializer).Deserialize(ref reader, ref context);
        }
    }
}