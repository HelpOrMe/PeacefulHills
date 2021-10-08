using System.Collections.Generic;
using Unity.Entities;

namespace PeacefulHills.Bootstrap.Worlds
{
    public static class WorldBootExtensions
    {
        public static void SetWorld(this IEnumerable<Boot> boots, World world)
        {
            foreach (Boot boot in boots)
            {
                if (boot is WorldChildBootstrap bootstrap)
                {
                    bootstrap.World = world;
                }

                boot.Children.SetWorld(world);
            }
        }
    }
}