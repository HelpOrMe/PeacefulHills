using Unity.Entities;

namespace PeacefulHills.ECS
{
    public struct ExtensionSingleton<TExtension> : IComponentData where TExtension : IWorldExtension
    {
        
    }
}