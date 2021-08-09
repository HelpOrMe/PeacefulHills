using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Testing
{
    public class EntityQueryMutable<TComponent0, TComponent1> : EntityQueryMutable<TComponent0> 
        where TComponent0: unmanaged, IComponentData
        where TComponent1: unmanaged, IComponentData
    {
        public EntityQueryMutable(EntityQuery query) : base(query) { }
        
        public static implicit operator TComponent1(EntityQueryMutable<TComponent0, TComponent1> queryMutable)
        {
            var array = (NativeArray<TComponent1>) queryMutable;
            TComponent1 component = array.Length > 0 ? array[0] : default;
            array.Dispose();
            return component;
        }

        public static implicit operator TComponent1[](EntityQueryMutable<TComponent0, TComponent1> queryMutable)
        {
            NativeArray<TComponent1> nativeArray = queryMutable;
            TComponent1[] array = nativeArray.ToArray();
            nativeArray.Dispose();
            return array;
        }
        
        public static implicit operator NativeArray<TComponent1>(EntityQueryMutable<TComponent0, TComponent1> queryMutable)
        {
            return queryMutable.Query.ToComponentDataArray<TComponent1>(Allocator.Temp);
        }
    }
}