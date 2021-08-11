using System;
using PeacefulHills.Extensions;
using PeacefulHills.Network;
using Unity.Entities;
using Unity.Jobs;
using Unity.Networking.Transport;

[assembly: RegisterGenericComponentType(typeof(ExtensionSingleton<INetworkDriverInfo>))]

namespace PeacefulHills.Network
{
    public interface INetworkDriverInfo : IWorldExtension, IDisposable
    {
        public NetworkDriver Current { get; }
        public NetworkDriver.Concurrent Concurrent { get; }

        public NetworkPipeline ReliablePipeline { get; }
        public NetworkPipeline UnreliablePipeline { get; }

        public JobHandle Dependency { get; set; }
    }
}