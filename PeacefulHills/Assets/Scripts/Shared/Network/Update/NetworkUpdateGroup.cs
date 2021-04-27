using Unity.Entities;

namespace PeacefulHills.Network
{
    [UpdateInGroup(typeof(NetworkSimulationGroup))]
    public class NetworkUpdateGroup : ComponentSystemGroup
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            RequireSingletonForUpdate<NetworkSingleton>();
        }

        protected override void OnUpdate()
        {
            NetworkHandle networkHandle = GetSingleton<NetworkSingleton>().Handle;
            var network = NetworkManager.GetNetwork<Network>(networkHandle);
         
            base.OnUpdate();
            
            network.LastDriverJobHandle.Complete();
            network.LastDriverJobHandle = network.Driver.ScheduleUpdate();
        }
    }
}