using System;
using PeacefulHills.ECS.World;
using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Network.Profiling
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    [AlwaysUpdateSystem]
    public class NetworkProfilerSystem : SystemBase
    {
        private NetworkProfilerSocket _profilerSocket;
        
        protected override void OnCreate()
        {
            _profilerSocket = new NetworkProfilerSocket();
        }

        protected override void OnUpdate()
        {
            var statsCollector = World.GetExtension<INetworkStatsCollector>();
            
            if (_profilerSocket.ConnectionCount > 0)
            {
                statsCollector.Entry("TestEntry").Write((byte)0xFF);
                
                foreach (byte[] bytes in statsCollector.IterateSendBytes())
                {  
                    _profilerSocket.Send(bytes);
                }
                
                statsCollector.Clear();
            }
        }
        
        protected override void OnDestroy()
        {
            _profilerSocket.Close();
            _profilerSocket.Dispose();
        }
    }
}