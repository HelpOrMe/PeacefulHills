using PeacefulHills.Network.Messages;
using Unity.Entities;

namespace PeacefulHills.Network.Tests
{
    public class TestMessageSystem : WriteMessageSystem<TestMessage, TestMessageSerializer>
    {
        protected override void OnUpdate()
        {
            HandleDependency(GetWriteJob().Schedule(MessagesQuery, Dependency));
        }
    }
}