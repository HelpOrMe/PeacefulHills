using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
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

                DataStreamReader reader = AsDataStreamReader(networkReceiveBuffer);

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

        private static unsafe DataStreamReader AsDataStreamReader<T>(DynamicBuffer<T> buffer)
            where T : struct, IBufferElementData
        {
            NativeArray<byte> array = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(
                buffer.GetUnsafePtr(), buffer.Length, Allocator.Invalid);

            #if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle safety = NativeArrayUnsafeUtility.GetAtomicSafetyHandle(buffer.AsNativeArray());
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref array, safety);
            #endif

            return new DataStreamReader(array);
        }
    }
}