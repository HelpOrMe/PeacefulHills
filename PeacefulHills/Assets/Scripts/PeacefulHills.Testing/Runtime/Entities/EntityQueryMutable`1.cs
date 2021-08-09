using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Testing
{
    public class EntityQueryMutable<TComponent> : EntityQueryMutable where TComponent: unmanaged, IComponentData
    {
        public EntityQueryMutable(EntityQuery query) : base(query) { }
        
        public static implicit operator TComponent(EntityQueryMutable<TComponent> queryMutable)
        {
            var array = (NativeArray<TComponent>) queryMutable;
            TComponent component = array.Length > 0 ? array[0] : default;
            array.Dispose();
            return component;
        }

        public static implicit operator TComponent[](EntityQueryMutable<TComponent> queryMutable)
        {
            NativeArray<TComponent> nativeArray = queryMutable;
            TComponent[] array = nativeArray.ToArray();
            nativeArray.Dispose();
            return array;
        }
        
        public static implicit operator NativeArray<TComponent>(EntityQueryMutable<TComponent> queryMutable)
        {
            return queryMutable.Query.ToComponentDataArray<TComponent>(Allocator.Temp);
        }
    }
}