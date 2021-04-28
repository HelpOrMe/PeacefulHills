using Unity.Entities;

namespace PeacefulHills.Network
{
    public struct NetworkSingleton : IComponentData
    {
        public NetworkHandle Handle;
    }
}