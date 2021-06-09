using Unity.Entities;

namespace PeacefulHills.Network.Connection
{
    [WriteGroup(typeof(NetworkConnectionWrapper))]
    public struct NetworkConnectionIgnored : IComponentData
    {
    }
}