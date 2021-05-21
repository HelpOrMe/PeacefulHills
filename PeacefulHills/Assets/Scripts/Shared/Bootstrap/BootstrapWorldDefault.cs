using Unity.Entities;

namespace PeacefulHills.Bootstrap
{
    public class BootstrapWorldDefault : BootstrapWorldBase
    {
        public override void Initialize()
        {
            var world = new World("Default world");

            world.AddSystems(Systems.All().Types());
            world.Loop();
            
            World.DefaultGameObjectInjectionWorld = world;
        }
    }
}