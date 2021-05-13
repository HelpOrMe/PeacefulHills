using PeacefulHills.ECS;
using Unity.Jobs;
using Unity.Networking.Transport;

namespace PeacefulHills.Network
{
    public interface INetwork : IWorldExtension
    {
        public NetworkDriver Driver { get; }
        public JobHandle LastDriverJobHandle { get; set; }
    }
}