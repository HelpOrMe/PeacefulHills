using PeacefulHills.Network;
using Unity.Entities;
using Unity.Networking.Transport;
using UnityEngine;

namespace PeacefulHills
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    public class EstablishClientConnection : SystemBase
    {
        private NetworkDriver _driver;
        private NetworkConnection _connection;

        private NetworkPipeline _reliablePipeline;
        private NetworkPipeline _unreliablePipeline;
        
        protected override void OnCreate()
        {
            _driver = NetworkDriver.Create();

            _reliablePipeline = _driver.CreatePipeline(typeof(ReliableSequencedPipelineStage));
            _unreliablePipeline = _driver.CreatePipeline(typeof(NullPipelineStage));
            
            NetworkEndPoint endpoint = NetworkEndPoint.LoopbackIpv4;
            endpoint.Port = 9000;
            
            _connection = _driver.Connect(endpoint);
        }

        protected override void OnDestroy()
        {
            _driver.Dispose();
            Dependency.Complete();
        }

        protected override void OnUpdate()
        {
            Dependency.Complete();
            
            DataStreamReader reader;
            NetworkEvent.Type cmd;
            
            while ((cmd = _driver.PopEventForConnection(_connection, out reader)) != NetworkEvent.Type.Empty)
            {
                switch (cmd)
                {
                    case NetworkEvent.Type.Data:
                        Debug.Log("Recv: " + reader.Length);
                        // Debug.Log("Raw: " + string.Join(" ", managedArray));
                        // Debug.Log("Hex: " + BitConverter.ToString(managedArray));
                        break;

                    case NetworkEvent.Type.Connect:
                        break;

                    case NetworkEvent.Type.Disconnect:
                        break;
                }
            }
            
            Dependency = _driver.ScheduleUpdate();
        }
    }
}