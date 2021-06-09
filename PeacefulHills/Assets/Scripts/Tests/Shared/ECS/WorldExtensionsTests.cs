using E7.EcsTesting;
using NUnit.Framework;
using PeacefulHills.ECS.World;
using PeacefulHills.Tests;
using Unity.Entities;

[assembly: RegisterGenericComponentType(typeof(ExtensionSingleton<WorldExtensionsTestWorld.ITestExtension>))]

namespace PeacefulHills.Tests
{
    public class WorldExtensionsTestWorld : WorldTestBase
    {
        [Test]
        public void SetExtensionPipeline()
        {
            w.SetExtension<ITestExtension>(new TestExtension());
            Assert.True(w.HasExtension<ITestExtension>());
            w.RemoveExtension<ITestExtension>();
            Assert.False(w.HasExtension<ITestExtension>());
        }

        [Test]
        public void RequestExtensionBeforeCreation()
        {
            bool requestInvoked = false;

            w.RequestExtension<ITestExtension>(extension => { requestInvoked = true; });

            Assert.False(requestInvoked);
            w.SetExtension<ITestExtension>(new TestExtension());
            Assert.True(requestInvoked);

            w.RemoveExtension<ITestExtension>();
        }

        [Test]
        public void RequestExtensionAfterCreation()
        {
            w.SetExtension<ITestExtension>(new TestExtension());

            bool requestInvoked = false;
            w.RequestExtension<ITestExtension>(extension => { requestInvoked = true; });

            Assert.True(requestInvoked);

            w.RemoveExtension<ITestExtension>();
        }

        [Test]
        public void RequireExtension()
        {
            var testSystem = new TestSystem();
            w.AddSystem(testSystem);
        }

        public interface ITestExtension : IWorldExtension
        {
        }

        public class TestExtension : ITestExtension
        {
        }

        [DisableAutoCreation]
        public class TestSystem : SystemBase
        {
            protected override void OnCreate()
            {
                this.RequireExtension<ITestExtension>();
            }

            protected override void OnUpdate()
            {
                Assert.True(HasSingleton<ExtensionSingleton<ITestExtension>>());
                Assert.True(World.HasExtension<ITestExtension>());
                World.DestroySystem(this);
            }
        }
    }
}