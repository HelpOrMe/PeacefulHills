using Unity.Entities;

namespace PeacefulHills.Network
{
    public struct ConnectionLink : IComponentData
    {
        /// <summary>
        /// Link to the real connection entity with <see cref="DriverConnection"/>.
        /// </summary>
        public Entity Entity;
    }
}