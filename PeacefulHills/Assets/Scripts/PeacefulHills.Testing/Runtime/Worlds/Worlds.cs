using System;
using Unity.Entities;

namespace PeacefulHills.Testing
{
    public static class Worlds
    {
        public static World Current
        {
            get
            {
                if (_now == null)
                {
                    throw new InvalidOperationException("No world selected, use Worlds.Select() to select a world.");
                }
                return _now;
            }
            private set => _now = value;
        }

        private static World _now;

        public static bool Exist(params string[] worldNames)
        {
            foreach (string worldName in worldNames)
            {
                if (!Exist(worldName))
                {
                    return false;
                }
            }

            return true;
        }
        
        public static bool Exist(string worldName)
        {
            foreach (World world in World.All)
            {
                if (world.Name == worldName)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        public static void Select(string worldName)
        {
            foreach (World world in World.All)
            {
                if (world.Name == worldName)
                {
                    Current = world;
                    return;
                }
            }

            throw new WorldsException($"Unable to select world. World with the name \"{worldName}\" does not exist!");
        }

        public static void Destroy()
        {
            World.DisposeAllWorlds();
        }

        public static void Destroy(params string[] worldNames)
        {
            foreach (string worldName in worldNames)
            {
                Destroy(worldName);
            }
        }
        
        public static void Destroy(string worldName)
        {
            foreach (World world in World.All)
            {
                if (world.Name == worldName)
                {
                    world.Dispose();
                    return;
                }
            }
        }
    }
}