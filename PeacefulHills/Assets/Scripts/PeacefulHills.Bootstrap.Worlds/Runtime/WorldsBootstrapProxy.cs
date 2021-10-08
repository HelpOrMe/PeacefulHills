using Unity.Entities;

namespace PeacefulHills.Bootstrap.Worlds
{
    public class WorldsBootstrapProxy : ICustomBootstrap
    {
        public static WorldsBootstrap Bootstrap;
        
        public bool Initialize(string defaultWorldName)
        {
            Bootstrap.CustomSetup();
            Bootstrap.Call();
            return true;
        }
    }
}