using PeacefulHills.Extensions;
using PeacefulHills.Network.Messages;
using Unity.Entities;

namespace PeacefulHills.Network.Tests
{
    [UpdateInGroup(typeof(MessagesInitializationGroup))]
    [AlwaysUpdateSystem]
    public class TestMessageRegistry : SystemBase
    {
        protected override void OnCreate()
        {
            World.RequestExtension<IMessageRegistry>(registry => registry.Register<TestMessage, TestMessageSerializer>());
        }

        protected override void OnUpdate()
        {
        }
    }
}