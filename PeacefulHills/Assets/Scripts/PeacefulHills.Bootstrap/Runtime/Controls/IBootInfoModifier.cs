using PeacefulHills.Bootstrap.Tree;

namespace PeacefulHills.Bootstrap
{
    public interface IBootInfoModifier : IBootControl
    {
        void ModifyInfo(ref BootInfo info);
    }
}