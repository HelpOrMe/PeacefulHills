using Unity.Entities;

namespace PeacefulHills.ECS.World
{
    // ReSharper disable once UnusedTypeParameter
    // Generic type is needed for uniqueness of the singleton component that will be registered with
    // RegisterGenericComponentType
    public struct ExtensionSingleton<TExtension> : IComponentData where TExtension : IWorldExtension
    {
    }
}