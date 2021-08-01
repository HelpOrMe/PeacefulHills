using System;
using PeacefulHills.ECS.World;
using PeacefulHills.Network;
using Unity.Entities;
using Unity.Jobs;
using Unity.Networking.Transport;

[assembly: RegisterGenericComponentType(typeof(ExtensionSingleton<INetwork>))]

namespace PeacefulHills.Network
{
    public interface INetwork : IWorldExtension, IDisposable
    {
        public NetworkDriver Driver { get; }
        public NetworkDriver.Concurrent DriverConcurrent { get; }
        
        public NetworkPipeline ReliablePipeline { get; }
        public NetworkPipeline UnreliablePipeline { get; }
     
        public JobHandle DriverDependency { get; set; }
    }
}