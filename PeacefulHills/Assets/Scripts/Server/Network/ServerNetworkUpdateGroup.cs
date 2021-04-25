using Unity.Entities;
using UnityEngine;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(ServerSimulationGroup))]
    public class ServerNetworkUpdateGroup : ComponentSystemGroup
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<NetworkSingleton>();
        }

        protected override void OnUpdate()
        {
            NetworkHandle networkHandle = GetSingleton<NetworkSingleton>().Handle;
            Network network = NetworkManager.GetNetwork(networkHandle);
         
            base.OnUpdate();
            
            network.LastDriverJobHandle.Complete();
            network.LastDriverJobHandle = network.Driver.ScheduleUpdate();
        }
    }
}