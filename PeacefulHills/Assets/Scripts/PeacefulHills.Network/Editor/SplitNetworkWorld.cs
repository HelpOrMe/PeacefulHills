// using System;
// using System.Collections.Generic;
// using System.Linq;
// using PeacefulHills.Bootstrap;
// using PeacefulHills.Bootstrap.Editor;
// using PeacefulHills.Bootstrap.Tree;
// using PeacefulHills.Bootstrap.Worlds;
// using Unity.Entities;
//
// namespace PeacefulHills.Network.Editor
// {
//     [BootOverride(typeof(NetworkWorld))]
//     public class SplitNetworkWorld : NetworkWorld, IBootConfigurable<SplitNetworkWorld.InitConfig>
//     {
//         private const string ClientAssemblyPattern = @"^(?!.*Client).*";
//         private const string ServerAssemblyPattern = @"^(?!.*Server).*";
//         
//         public InitConfig Config { get; set; }
//         
//         public class InitConfig
//         {
//             public bool SplitWorlds = true;
//             public int ClientCount  = 1;
//         }
//         
//         public override void Invoke()
//         {
//             if (!Config.SplitWorlds)
//             {
//                 // base.Invoke();
//                 return;
//             }
//             
//             List<Type> clientSystems = SystemTypes.FilterAssembly(ServerAssemblyPattern).ToList();
//             List<Type> serverSystems = SystemTypes.FilterAssembly(ClientAssemblyPattern).ToList();
//
//             var worldBranches = new List<IBootBranch>();
//             
//             for (int i = 0; i < Config.ClientCount; i++)
//             {
//                 World clientWorld = CreateWorld($"Client world №{i + 1}", clientSystems);
//                 IBootBranch clientBranch = CreateBranch(ClientAssemblyPattern);
//                 clientBranch.PropagateWorld(clientWorld);
//                 worldBranches.Add(clientBranch);
//             }
//     
//             World serverWorld = CreateWorld("Server world", serverSystems);
//             IBootBranch serverBranch = CreateBranch(ServerAssemblyPattern);
//             serverBranch.PropagateWorld(serverWorld);
//             worldBranches.Add(serverBranch);
//             
//             Root.Children.Clear();
//             foreach (IBootBranch branch in worldBranches)
//             {
//                 Root.Children.Add(branch);
//                 branch.Invoke();
//             }
//         }
//         
//         private World CreateWorld(string worldName, IEnumerable<Type> systems)
//         {
//             var world = new World(worldName);
//             world.AddSystems(systems);
//             world.Loop();
//             return world;
//         }
//
//         private IBootBranch CreateBranch(string assemblyPattern)
//         {
//             IBootBranch newBranch = ManualBranch.WithChildren(Root.Children).Clone();
//             newBranch.ScrubTree(child => !child.Boot.Type.MatchAssembly(assemblyPattern));
//             return newBranch;
//         }
//     }
// }
