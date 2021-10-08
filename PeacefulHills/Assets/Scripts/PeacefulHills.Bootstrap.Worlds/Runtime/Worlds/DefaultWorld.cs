using Unity.Entities;

namespace PeacefulHills.Bootstrap.Worlds
{
    public class DefaultWorld : WorldBootstrap
    {
        protected override void Act()
        {
            var world = new World("Default world");
            
            world.AddSystems(Systems);
            world.Loop();

            World.DefaultGameObjectInjectionWorld = world;
            Children.SetWorld(world);
        }
    }
}