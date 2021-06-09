using PeacefulHills.ECS.World;
using PeacefulHills.Network;
using Unity.Entities;
using Unity.Jobs;
using Unity.Networking.Transport;

[assembly: RegisterGenericComponentType(typeof(ExtensionSingleton<INetwork>))]

namespace PeacefulHills.Network
{
    public interface INetwork : IWorldExtension
    {
        public NetworkDriver Driver { get; }
        public NetworkDriver.Concurrent DriverConcurrent { get; set; }
        public JobHandle DriverDependency { get; set; }
    }
}