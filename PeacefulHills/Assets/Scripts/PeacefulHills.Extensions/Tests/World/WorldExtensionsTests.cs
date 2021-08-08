using NUnit.Framework;
using PeacefulHills.Extensions;
using PeacefulHills.Extensions.Tests;
using PeacefulHills.Testing;
using Unity.Entities;

[assembly: RegisterGenericComponentType(typeof(ExtensionSingleton<WorldExtensionsTests.ITestExtension>))]

namespace PeacefulHills.Extensions.Tests
{
    public class WorldExtensionsTests
    {
        [SetUp]
        public void Setup()
        {
            Worlds.Select("Default world");
        }
        
        [Test]
        public void SetExtensionPipeline()
        {
            Worlds.Now.SetExtension<ITestExtension>(new TestExtension());
            Assert.True(Worlds.Now.HasExtension<ITestExtension>());
            Worlds.Now.RemoveExtension<ITestExtension>();
            Assert.False(Worlds.Now.HasExtension<ITestExtension>());
        }

        [Test]
        public void RequestExtensionBeforeCreation()
        {
            bool requestInvoked = false;

            Worlds.Now.RequestExtension<ITestExtension>(extension => { requestInvoked = true; });

            Assert.False(requestInvoked);
            Worlds.Now.SetExtension<ITestExtension>(new TestExtension());
            Assert.True(requestInvoked);

            Worlds.Now.RemoveExtension<ITestExtension>();
        }

        [Test]
        public void RequestExtensionAfterCreation()
        {
            Worlds.Now.SetExtension<ITestExtension>(new TestExtension());

            bool requestInvoked = false;
            Worlds.Now.RequestExtension<ITestExtension>(extension => { requestInvoked = true; });

            Assert.True(requestInvoked);

            Worlds.Now.RemoveExtension<ITestExtension>();
        }

        [Test]
        public void RequireExtension()
        {
            var testSystem = new TestSystem();
            Worlds.Now.AddSystem(testSystem);
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