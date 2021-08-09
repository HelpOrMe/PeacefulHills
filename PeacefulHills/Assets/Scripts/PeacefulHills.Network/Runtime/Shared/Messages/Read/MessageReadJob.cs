using PeacefulHills.Extensions;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Messages
{
    [BurstCompile]
    public struct MessageReadJob : IJobEntityBatch
    {
        [ReadOnly] public EntityTypeHandle EntityHandle;
        [ReadOnly] public BufferTypeHandle<NetworkReceiveBufferPool> PoolHandle;
        [NativeDisableParallelForRestriction] public BufferFromEntity<NetworkReceiveBuffer> ReceiveBufferFromEntity;

        [ReadOnly] public NativeList<MessageInfo> Messages;
        public EntityCommandBuffer.ParallelWriter CommandBuffer;

        public void Execute(ArchetypeChunk batchInChunk, int batchIndex)
        {
            BufferAccessor<NetworkReceiveBufferPool> pools = batchInChunk.GetBufferAccessor(PoolHandle);
            NativeArray<Entity> entities = batchInChunk.GetNativeArray(EntityHandle);

            for (int i = 0; i < batchInChunk.Count; i++)
            {
                DynamicBuffer<NetworkReceiveBufferPool> pool = pools[i];

                Entity receiveBufferEntity = pool[(int) NetworkPackageType.Message].Entity;
                DynamicBuffer<NetworkReceiveBuffer> networkReceiveBuffer = ReceiveBufferFromEntity[receiveBufferEntity];

                var reader = new DataStreamReader(networkReceiveBuffer.AsBytes());

                while (reader.GetBytesRead() < reader.Length)
                {
                    int messageId = reader.ReadUShort();
                    MessageInfo messageInfo = Messages[messageId];
                    var context = new MessageDeserializerContext
                    {
                        CommandBuffer = CommandBuffer,
                        Connection = entities[i],
                        SortKey = batchIndex
                    };

                    messageInfo.Deserialize.Invoke(ref reader, ref context);
                }

                networkReceiveBuffer.Clear();
            }
        }
    }
}