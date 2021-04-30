using Unity.Entities;

namespace PeacefulHills.Network
{
    public static class SystemsExtension
    {
        public static Network GetNetworkFromSingleton(this ComponentSystemBase system) 
            => NetworkManager<Network>.GetNetwork(system.GetSingleton<NetworkSingleton>().Handle);
        
        public static TNetwork GetNetworkFromSingleton<TNetwork>(this ComponentSystemBase system) 
            where TNetwork : struct, INetwork
            => NetworkManager<TNetwork>.GetNetwork(system.GetSingleton<NetworkSingleton>().Handle);
    }
}