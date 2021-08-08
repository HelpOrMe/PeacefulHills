using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Testing
{
    public class EntityQueryMutable<TComponent> : EntityQueryMutable where TComponent: unmanaged, IComponentData
    {
        public EntityQueryMutable(EntityQuery query) : base(query) { }
        
        public static implicit operator TComponent(EntityQueryMutable<TComponent> entityQueryMutable)
        {
            var array = (TComponent[]) entityQueryMutable;
            return array.Length > 0 ? array[0] : default;
        }

        public static implicit operator TComponent[](EntityQueryMutable<TComponent> entityQueryMutable)
        {
            NativeArray<TComponent> nativeArray = entityQueryMutable;
            TComponent[] array = nativeArray.ToArray();
            nativeArray.Dispose();
            return array;
        }
        
        public static implicit operator NativeArray<TComponent>(EntityQueryMutable<TComponent> entityQueryMutable)
        {
            return entityQueryMutable.Query.ToComponentDataArray<TComponent>(Allocator.Temp);
        }
    }
}