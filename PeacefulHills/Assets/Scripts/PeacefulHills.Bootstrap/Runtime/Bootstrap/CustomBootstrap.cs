using Unity.Entities;

namespace PeacefulHills.Bootstrap
{
    public class CustomBootstrap : ICustomBootstrap
    {
        public bool Initialize(string defaultWorldName)
        {
            WorldBootstraps.Initialize();
            return true;
        }
    }
}