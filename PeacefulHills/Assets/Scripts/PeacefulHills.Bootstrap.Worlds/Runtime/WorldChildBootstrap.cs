using Unity.Entities;

namespace PeacefulHills.Bootstrap.Worlds
{
    public abstract class WorldChildBootstrap : Bootstrap
    {
        public World World { get; set; }
    }
}