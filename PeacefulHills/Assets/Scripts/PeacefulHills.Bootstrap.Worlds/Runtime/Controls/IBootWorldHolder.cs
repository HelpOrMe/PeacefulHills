using Unity.Entities;

namespace PeacefulHills.Bootstrap.Worlds
{
    public interface IBootWorldHolder
    {
        World World { get; set; }
    }
}