using System.Collections.Generic;
using Unity.Entities;

namespace PeacefulHills.Network.Packet
{
    [UpdateInGroup(typeof(NetworkInitializationGroup))]
    [UpdateAfter(typeof(PacketPreInitializationSystem))]
    [UpdateBefore(typeof(PacketPostInitializationSystem))]
    public class PacketRegistryInitializationGroup : ComponentSystemGroup
    {
        protected class SystemTypeComparer : IComparer<ComponentSystemBase>
        {
            public int Compare(ComponentSystemBase x, ComponentSystemBase y)
            {
                int firstHash = x?.GetType().FullName?.GetHashCode() ?? 0;
                int secondHash = x?.GetType().FullName?.GetHashCode() ?? 0;
                return secondHash - firstHash;
            }
        }
        
        protected override void OnCreate()
        {
            base.OnCreate();
            
            EnableSystemSorting = false;

            // todo: think about another way of statically sort
            var systems = (List<ComponentSystemBase>)Systems;
            systems.Sort(new SystemTypeComparer());
        }
    }
}