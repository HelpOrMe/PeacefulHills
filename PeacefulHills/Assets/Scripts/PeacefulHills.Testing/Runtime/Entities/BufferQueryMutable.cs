using Unity.Entities;

namespace PeacefulHills.Testing
{
    public class BufferQueryMutable<TBuffer> : EntityQueryMutable where TBuffer: unmanaged, IBufferElementData
    {
        public DynamicBuffer<TBuffer> Buffer => this;
        
        public BufferQueryMutable(EntityQuery query) : base(query) { }

        public static implicit operator DynamicBuffer<TBuffer>(BufferQueryMutable<TBuffer> bufferQueryMut)
        {
            return Worlds.Now.EntityManager.GetBuffer<TBuffer>(bufferQueryMut);
        }
    }
}