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

        public static implicit operator Entity(EntityQueryMutable entityQueryMutable)
        {
            var array = (Entity[]) entityQueryMutable;
            return array.Length > 0 ? array[0] : Entity.Null;
        }

        public static implicit operator Entity[](EntityQueryMutable entityQueryMutable)
        {
            NativeArray<Entity> nativeArray = entityQueryMutable;
            Entity[] array = nativeArray.ToArray();
            nativeArray.Dispose();
            return array;
        }
        
        public static implicit operator NativeArray<Entity>(EntityQueryMutable entityQueryMutable)
        {
            return entityQueryMutable.Query.ToEntityArray(Allocator.Temp);
        }
        
        public static implicit operator EntityQuery(EntityQueryMutable entityQueryMutable)
        {
            return entityQueryMutable.Query;
        }
        
        public static implicit operator bool(EntityQueryMutable entityQueryMutable)
        {
            return !entityQueryMutable.Query.IsEmpty;
        }
    }
}