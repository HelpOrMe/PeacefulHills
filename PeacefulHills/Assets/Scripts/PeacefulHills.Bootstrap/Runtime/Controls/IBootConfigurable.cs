namespace PeacefulHills.Bootstrap
{
    public interface IBootConfigurable<TConfig> : IBootControl where TConfig : struct
    {
        TConfig Config { get; set; }
    }
}