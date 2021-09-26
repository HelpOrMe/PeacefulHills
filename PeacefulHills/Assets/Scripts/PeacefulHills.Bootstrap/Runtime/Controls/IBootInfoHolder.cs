using PeacefulHills.Bootstrap.Tree;

namespace PeacefulHills.Bootstrap
{
    public interface IBootInfoHolder : IBootControl
    {
        public BootInfo Boot { get; set; }
    }
}