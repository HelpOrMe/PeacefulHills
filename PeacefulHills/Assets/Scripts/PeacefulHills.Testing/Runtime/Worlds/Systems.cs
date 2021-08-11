using Unity.Entities;

namespace PeacefulHills.Testing
{
    public static class Systems
    {
        public static void Disable<TSystem>() where TSystem : SystemBase
        {
            Worlds.Current.GetOrCreateSystem<TSystem>().Enabled = false;
        }

        public static void Enable<TSystem>() where TSystem : SystemBase
        {
            Worlds.Current.GetOrCreateSystem<TSystem>().Enabled = true;
        }
    }
}