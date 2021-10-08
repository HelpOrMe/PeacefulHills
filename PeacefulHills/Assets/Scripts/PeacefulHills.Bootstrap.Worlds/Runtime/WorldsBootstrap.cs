using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.Entities;
using PeacefulHills.Extensions;

namespace PeacefulHills.Bootstrap.Worlds
{
    public class WorldsBootstrap : Bootstrap
    {
        public WorldsBootstrap()
        {
            WorldsBootstrapProxy.Bootstrap = this;
        }
        
        /// <summary>
        /// Prevent the default installation of instructions so that the chain of calls does not spread to us
        /// and our children. CustomSetup() and Call() will be called from the ICustomBootstrap implementation
        /// (<see cref="WorldsBootstrapProxy"/>). 
        /// </summary>
        public override void Setup()
        {
        }
        
        public void CustomSetup()
        {
            Instructions.Add(new ActInstruction());
            Instructions.Add(new CallChildrenInstruction());
        }

        protected override void Act()
        {
            Dictionary<Type, List<Type>> worldsSystems = GatherWorldsSystems(); 
            
            foreach (Boot child in Children)
            {
                Type childType = child.GetType();
                
                if (child is WorldBootstrap worldBoot && worldsSystems.ContainsKey(childType))
                {
                    worldBoot.Systems = worldsSystems[childType];
                }
            }
        }

        public Dictionary<Type, List<Type>> GatherWorldsSystems()
        {
            IReadOnlyList<Type> systems = TypeManager.GetSystems(WorldSystemFilterFlags.Default);
            
            var systemsParent = new Dictionary<Type, Type>();
            foreach (Type system in systems)
            {
                Type parent = system.GetCustomAttribute<UpdateInGroupAttribute>()?.GroupType;
                systemsParent[system] = parent ?? typeof(SimulationSystemGroup); 
            }

            var worldsSystems = new Dictionary<Type, List<Type>>
            {
                [typeof(DefaultWorld)] = new List<Type>()
            };
            
            foreach (Type system in systems)
            {
                Type world = GetRequiredWorld(system, systemsParent);
                
                if (!worldsSystems.ContainsKey(world))
                {
                    worldsSystems[world] = new List<Type>();
                    
                }
                
                worldsSystems[world].Add(system);
            }
            
            return worldsSystems;
        }

        private Type GetRequiredWorld(Type system, Dictionary<Type, Type> systemsParent)
        {
            for (Type parent = system; 
                systemsParent.ContainsKey(parent); 
                parent = systemsParent[parent])
            {
                Type world = parent.GetCustomAttribute<UpdateInWorldAttribute>(true)?.Type;

                if (world != null)
                {
                    return world;
                }
            }

            return typeof(DefaultWorld);
        }
    }
}