using DryIoc;

using JetBrains.Annotations;

using NUnit.Framework;

namespace LVK.DryIoc.Tests
{
    [TestFixture]
    public class ContainerExtensionsTests
    {
        [SetUp]
        public void SetUp()
        {
            DummyServicesBootstrapper.BootstrapCallCounter = 0;
        }

        [Test]
        public void Bootstrap_CalledOnce_CallsBootstrapMethod()
        {
            var container = new Container();

            container.Bootstrap<DummyServicesBootstrapper>();
            
            Assert.That(DummyServicesBootstrapper.BootstrapCallCounter, Is.EqualTo(1));
        }

        [Test]
        public void Bootstrap_CalledTwice_CallsBootstrapMethodOnlyOnce()
        {
            var container = new Container();

            container.Bootstrap<DummyServicesBootstrapper>();
            container.Bootstrap<DummyServicesBootstrapper>();

            Assert.That(DummyServicesBootstrapper.BootstrapCallCounter, Is.EqualTo(1));
        }
    }

    [UsedImplicitly]
    public class DummyServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            BootstrapCallCounter++;
        }

        public static int BootstrapCallCounter { get; set; }
    }
}