using Unity.Entities;

namespace PeacefulHills.Network
{
    public static class SystemsExtension
    {
        public static Network GetNetworkFromSingleton(this ComponentSystemBase system) 
            => NetworkManager.GetNetwork(system.GetSingleton<NetworkSingleton>().Handle);
        
        public static TNetwork GetNetworkFromSingleton<TNetwork>(this ComponentSystemBase system) 
            where TNetwork : Network
            => NetworkManager.GetNetwork<TNetwork>(system.GetSingleton<NetworkSingleton>().Handle);
    }
}