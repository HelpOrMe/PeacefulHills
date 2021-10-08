namespace PeacefulHills.Bootstrap.Editor
{
    /// <summary>
    /// Adds the ability to configure a boot from the editor.
    /// </summary>
    public interface IBootConfigurable<TConfig> where TConfig : new()
    {
        TConfig Config { get; set; }
    }
}
