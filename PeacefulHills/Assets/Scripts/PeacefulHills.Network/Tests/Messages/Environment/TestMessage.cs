using PeacefulHills.Network.Messages;
using Unity.Entities;

namespace PeacefulHills.Network.Tests
{
    public struct TestMessage : IMessage, IComponentData
    {
        public int Value;
    }
}