using DryIoc;

using NUnit.Framework;

namespace LVK.DryIoc.Tests
{
    [TestFixture]
    public class ServicesBootstrapperRegisterTests
    {
        [SetUp]
        public void SetUp()
        {
            DummyBootstrapper.Reset();
        }

        [Test]
        public void Bootstrap_SameBootstrapperRegisteredTwice_OnlyCallsBootstrapOnce()
        {
            var container = new Container();
            
            container.Bootstrap<DummyBootstrapper>();
            container.Bootstrap<DummyBootstrapper>();

            Assert.That(DummyBootstrapper.BootstrapCallCounter, Is.EqualTo(1));
        }
    }
}