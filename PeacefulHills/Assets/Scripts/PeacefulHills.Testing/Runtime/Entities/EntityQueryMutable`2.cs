using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Testing
{
    public class EntityQueryMutable<TComponent0, TComponent1> : EntityQueryMutable<TComponent0> 
        where TComponent0: unmanaged, IComponentData
        where TComponent1: unmanaged, IComponentData
    {
        public EntityQueryMutable(EntityQuery query) : base(query) { }
        
        public static implicit operator TComponent1(EntityQueryMutable<TComponent0, TComponent1> entityQueryMutable)
        {
            var array = (TComponent1[]) entityQueryMutable;
            return array.Length > 0 ? array[0] : default;
        }

        public static implicit operator TComponent1[](EntityQueryMutable<TComponent0, TComponent1> entityQueryMutable)
        {
            NativeArray<TComponent1> nativeArray = entityQueryMutable;
            TComponent1[] array = nativeArray.ToArray();
            nativeArray.Dispose();
            return array;
        }
        
        public static implicit operator NativeArray<TComponent1>(EntityQueryMutable<TComponent0, TComponent1> entityQueryMutable)
        {
            return entityQueryMutable.Query.ToComponentDataArray<TComponent1>(Allocator.Temp);
        }
    }
}