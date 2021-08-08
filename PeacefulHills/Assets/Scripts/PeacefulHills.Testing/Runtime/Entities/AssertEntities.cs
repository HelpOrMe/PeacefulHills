using NUnit.Framework;
using Unity.Entities;

namespace PeacefulHills.Testing
{
    public static class AssertEntities
    {
        public static void Components<TComponent0, TComponent1, TComponent2>(Entity entity) 
            where TComponent0 : IComponentData
            where TComponent1 : IComponentData
            where TComponent2 : IComponentData
        {
            Components(entity, typeof(TComponent0), typeof(TComponent1), typeof(TComponent2));
        }

        public static void Components<TComponent0, TComponent1>(Entity entity) 
            where TComponent0 : IComponentData
            where TComponent1 : IComponentData
        {
            Components(entity, typeof(TComponent0), typeof(TComponent1));
        }
        
        public static void Components<TComponent>(Entity entity) 
            where TComponent : IComponentData
        {
            Components(entity, typeof(TComponent));
        }

        public static void Components(Entity entity, params ComponentType[] components)
        {
            Assert.True(entity.HasComponents(components));
        }
    }
}