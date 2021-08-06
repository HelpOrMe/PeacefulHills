using Unity.Entities;

namespace PeacefulHills.Bootstrap
{
    public class CustomBootstrapProxy : ICustomBootstrap
    {
        public bool Initialize(string defaultWorldName)
        {
            WorldsBootstrap.Initialize();
            return true;
        }
    }
}