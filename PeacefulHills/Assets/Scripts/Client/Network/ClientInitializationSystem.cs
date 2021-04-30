// using Unity.Entities;
// using Unity.Networking.Transport;
// using Unity.Networking.Transport.Utilities;
//
// namespace PeacefulHills.Network
// {
//     public class ClientInitializationSystem : SystemBase
//     {
//         protected override void OnCreate()
//         {
//             // InitializeNetworkSingleton();
//         }
//         
//         private void InitializeNetworkSingleton()
//         {
//             NetworkHandle networkHandle = NetworkManager.AddNetwork(InitializeNetwork());
//             Entity entity = EntityManager.CreateEntity(typeof(NetworkSingleton));
//             EntityManager.SetComponentData(entity, new NetworkSingleton {Handle = networkHandle});
//         }
//
//         private Network InitializeNetwork()
//         {
//             var network = new ClientNetwork();
//          
//             var reliabilityParams = new ReliableUtility.Parameters { WindowSize = 32 };
//             var fragmentationParams = new FragmentationUtility.Parameters { PayloadCapacity = 16 * 1024 };
//             
//             network.Driver = NetworkDriver.Create(reliabilityParams, fragmentationParams);
//             network.ReliablePipeline = network.Driver.CreatePipeline(typeof(ReliableSequencedPipelineStage));
//             
//             NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
//             endpoint.Port = 9000;
//             
//             NetworkConnection connection = network.Driver.Connect(endpoint);
//
//             Entity entity = EntityManager.CreateEntity(typeof(NetworkStreamConnection));
//             EntityManager.SetComponentData(entity, new NetworkStreamConnection { Connection = connection });
//             
//             return network;
//         }
//
//         protected override void OnUpdate() { }
//     }
// }