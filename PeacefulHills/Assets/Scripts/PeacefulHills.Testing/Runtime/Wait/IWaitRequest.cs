using Unity.Entities;

namespace PeacefulHills.Testing
{
    public interface IWaitRequest
    {
        bool Update(World world);
    }
}