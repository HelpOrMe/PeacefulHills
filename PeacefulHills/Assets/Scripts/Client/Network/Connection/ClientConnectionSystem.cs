using Unity.Entities;
using Unity.Networking.Transport;

namespace PeacefulHills.Network.Connection
{
    public class ClientConnectionSystem : SystemBase
    {
        private ClientInitializationSystem _clientInitialization;
        
        protected override void OnCreate()
        {
            _clientInitialization = World.GetOrCreateSystem<ClientInitializationSystem>();
        }

        protected override void OnUpdate()
        {
            
        }
    }
}