// using PeacefulHills.Extensions;
// using PeacefulHills.Network.Profiling;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Networking.Transport;
//
// namespace PeacefulHills.Network
// {
//     // todo: del
//     [UpdateInGroup(typeof(NetworkInitializationGroup))]
//     [UpdateAfter(typeof(ConnectionInitializationSystem))]
//     public class EstablishClientConnection : SystemBase
//     {
//         protected override void OnCreate()
//         {
//             World.RequestExtension<INetworkDriverInfo>(
//                 driver => World.RequestExtension<ConnectionBuilder>(
//                     builder => EstablishConnection(driver, builder)));
//         }
//
//         private void EstablishConnection(INetworkDriverInfo driver, ConnectionBuilder connectionBuilder)
//         {
//             NetworkEndPoint endpoint = NetworkEndPoint.LoopbackIpv4;
//             endpoint.Port = 9000;
//             
//             var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
//
//             if (NetworkWorldsInitSettings.Current.SplitWorlds)
//             {
//                 NetworkConnection connection = driver.Current.Connect(endpoint);
//                 connectionBuilder.CreateConnection(commandBuffer, connection);
//             }
//             else
//             {
//                 connectionBuilder.CreateHostConnection(commandBuffer, default);
//             }
//             
//             commandBuffer.Playback(EntityManager);
//         }
//         
//         protected override void OnUpdate()
//         {
//         }
//     }
// }