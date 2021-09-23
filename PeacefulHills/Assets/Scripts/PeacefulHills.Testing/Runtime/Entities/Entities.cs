using Unity.Entities;

namespace PeacefulHills.Testing
{
    public static class Entities
    {
        public static EntityQueryMutable<TComponent> Component<TComponent>() 
            where TComponent : unmanaged, IComponentData
        {
            EntityQuery entityQuery = Worlds.Current.EntityManager.CreateEntityQuery(typeof(TComponent));
            return new EntityQueryMutable<TComponent>(entityQuery);
        }
        
        public static EntityQueryMutable<TComponent0, TComponent1> Q<TComponent0, TComponent1>()
            where TComponent0 : unmanaged, IComponentData
            where TComponent1 : unmanaged, IComponentData
        {
            EntityQuery entityQuery = Worlds.Current.EntityManager.CreateEntityQuery(
                typeof(TComponent0), typeof(TComponent1));
            return new EntityQueryMutable<TComponent0, TComponent1>(entityQuery);
        }
        
        public static BufferQueryMutable<TBuffer> Buffer<TBuffer>() 
            where TBuffer : unmanaged, IBufferElementData
        {
            EntityQuery entityQuery = Worlds.Current.EntityManager.CreateEntityQuery(typeof(TBuffer));
            return new BufferQueryMutable<TBuffer>(entityQuery);
        }
        
        public static BufferQueryMutable<TBuffer> Buffer<TBuffer, TTag>() 
            where TBuffer : unmanaged, IBufferElementData
            where TTag : unmanaged, IComponentData
        {
            EntityQuery entityQuery = Worlds.Current.EntityManager.CreateEntityQuery(
                typeof(TBuffer), typeof(TTag));
            
            return new BufferQueryMutable<TBuffer>(entityQuery);
        }
    }
}