using PeacefulHills.Bootstrap;
using Unity.Entities;

namespace PeacefulHills.Network
{
    public class NetworkWorld : WorldBootstrapBase
    {
        public override void Initialize()
        {
            var world = new World("Network world");
            world.AddSystems(Systems.Tree().Types());
            world.Loop();
        }
    }
}