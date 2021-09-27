namespace PeacefulHills.Bootstrap.Editor
{
    /// <summary>
    /// Adds the ability to configure the bootstrap from the editor.
    /// </summary>
    public interface IBootConfigurable<TConfig> : IBootControl where TConfig : new()
    {
        TConfig Config { get; set; }
    }
}
