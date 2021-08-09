using Unity.Collections;
using Unity.Entities;

namespace PeacefulHills.Testing
{
    public class EntityQueryMutable
    {
        protected EntityQuery Query;

        public EntityQueryMutable(EntityQuery query)
        {
            Query = query;
        }

        public static implicit operator Entity(EntityQueryMutable queryMutable)
        {
            var array = (NativeArray<Entity>) queryMutable;
            if (array.Length == 0)
            {
                throw new EntitiesException($"No entities were found.");
            }
            
            Entity entity = array[0];
            array.Dispose();
            return entity;
        }

        public static implicit operator Entity[](EntityQueryMutable queryMutable)
        {
            NativeArray<Entity> nativeArray = queryMutable;
            Entity[] array = nativeArray.ToArray();
            nativeArray.Dispose();
            return array;
        }
        
        public static implicit operator NativeArray<Entity>(EntityQueryMutable queryMutable)
        {
            return queryMutable.Query.ToEntityArray(Allocator.Temp);
        }
        
        public static implicit operator EntityQuery(EntityQueryMutable queryMutable)
        {
            return queryMutable.Query;
        }
        
        public static implicit operator bool(EntityQueryMutable queryMutable)
        {
            return !queryMutable.Query.IsEmpty;
        }
    }
}