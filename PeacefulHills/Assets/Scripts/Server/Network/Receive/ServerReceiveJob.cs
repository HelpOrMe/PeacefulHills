using System;
using Unity.Assertions;
using Unity.Collections;
using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Receive
{
    public struct ServerReceiveJob : IJobParallelForDefer
    {
        public NetworkDriver.Concurrent Driver;
        [ReadOnly] public NativeArray<NetworkConnection> Connections;
        
        public void Execute(int index)
        {
            Assert.IsTrue(Connections[index].IsCreated);

            while (PopEvent(Connections[index], out NetworkEvent.Type eventType, out DataStreamReader reader))
            {
                if (eventType == NetworkEvent.Type.Data)
                {
                    ReceiveData(ref reader);
                }
                
                else if (eventType == NetworkEvent.Type.Disconnect)
                {
                    Connections[index] = default;
                }
            }
        }
        
        private bool PopEvent(NetworkConnection connection, out NetworkEvent.Type eventType,
            out DataStreamReader reader)
        {
            eventType = Driver.PopEventForConnection(connection, out reader);
            return eventType != NetworkEvent.Type.Disconnect;
        }

        private void ReceiveData(ref DataStreamReader reader)
        {
            switch ((NetworkStreamType) reader.ReadByte())
            {
                case NetworkStreamType.Chunk:
                    ReceiveChunkData(ref reader);
                    break;
                
                case NetworkStreamType.Message:
                    ReceiveMessageData(ref reader);
                    break;

#if ENABLE_UNITY_COLLECTIONS_CHECKS
                default:
                    throw new InvalidOperationException("Received unknown message type");
#endif
            }
        }

        private void ReceiveChunkData(ref DataStreamReader reader)
        {
            throw new NotImplementedException();
        }

        private void ReceiveMessageData(ref DataStreamReader reader)
        {
            throw new NotImplementedException();
        }
    }
}