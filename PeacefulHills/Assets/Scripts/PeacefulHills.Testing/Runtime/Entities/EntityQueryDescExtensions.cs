using Unity.Entities;

namespace PeacefulHills.Testing
{
    public static class EntityQueryDescExtensions
    {
        public static EntityQueryDesc WithAll(this EntityQueryDesc desc, params ComponentType[] all)
        {
            desc.All = all;
            return desc;
        }

        public static EntityQueryDesc WithAny(this EntityQueryDesc desc, params ComponentType[] any)
        {
            desc.Any = any;
            return desc;
        }
        
        public static EntityQueryDesc WithNone(this EntityQueryDesc desc, params ComponentType[] none)
        {
            desc.None = none;
            return desc;
        }
        
        public static EntityQueryDesc WithOptions(this EntityQueryDesc desc, EntityQueryOptions options)
        {
            desc.Options = options;
            return desc;
        }
    }
}