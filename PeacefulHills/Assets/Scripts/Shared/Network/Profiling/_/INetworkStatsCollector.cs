using System;
using System.Collections.Generic;
using PeacefulHills.ECS.World;
using PeacefulHills.Network.Profiling;
using Unity.Entities;

[assembly: RegisterGenericComponentType(typeof(ExtensionSingleton<INetworkStatsCollector>))]

namespace PeacefulHills.Network.Profiling
{
    public interface INetworkStatsCollector : IWorldExtension, IDisposable
    {
        public NetworkStatsEntry Entry(string name);

        public IEnumerable<byte[]> IterateSendBytes();
        
        public void Clear();
    }
}