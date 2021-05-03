using Unity.Entities;

namespace PeacefulHills.Network
{
    public static class SystemsExtension
    {
        public static TNetwork GetNetworkFromSingleton<TNetwork>(this ComponentSystemBase system) 
            where TNetwork : INetwork
            => (TNetwork)system.GetNetworkFromSingleton();

        public static INetwork GetNetworkFromSingleton(this ComponentSystemBase system) 
            => NetworkManager.GetNetwork(system.GetSingleton<NetworkSingleton>().Handle);
    }
}