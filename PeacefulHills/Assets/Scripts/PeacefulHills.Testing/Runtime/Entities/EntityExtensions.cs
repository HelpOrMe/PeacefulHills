using NUnit.Framework;
using Unity.Entities;

namespace PeacefulHills.Testing
{
    public static class EntityExtensions
    {
        public static bool HasComponents<TComponent0, TComponent1, TComponent2>(this Entity entity) 
            where TComponent0 : IComponentData
            where TComponent1 : IComponentData
            where TComponent2 : IComponentData
        {
            return entity.HasComponents(typeof(TComponent0), typeof(TComponent1), typeof(TComponent2));
        }

        public static bool HasComponents<TComponent0, TComponent1>(this Entity entity) 
            where TComponent0 : IComponentData
            where TComponent1 : IComponentData
        {
            return entity.HasComponents(typeof(TComponent0), typeof(TComponent1));
        }
        
        public static bool HasComponents<TComponent>(this Entity entity) 
            where TComponent : IComponentData
        {
            return entity.HasComponents(typeof(TComponent));
        }
        
        public static bool HasComponents(this Entity entity, params ComponentType[] components)
        {
            foreach (ComponentType component in components)
            {
                if (!Worlds.Now.EntityManager.HasComponent(entity, component))
                {
                    return false;
                }
            }
            return true;
        }

        public static TComponent Component<TComponent>(this Entity entity) 
            where TComponent : unmanaged, IComponentData
        {
            return Worlds.Now.EntityManager.GetComponentData<TComponent>(entity);
        }
    }
}