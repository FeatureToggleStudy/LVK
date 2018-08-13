using System;

using DryIoc;

using NodaTime;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.Core.Services.Tests
{
    [TestFixture]
    public class ServicesBootstrapperTests
    {
        [Test]
        public void Bootstrap_NullContainer_ThrowsArgumentNullException()
        {
            var bootstrapper = new ServicesBootstrapper();
            Assert.Throws<ArgumentNullException>(() => bootstrapper.Bootstrap(null));
        }

        [Test]
        public void Bootstrap_WithContainer_RegistersClock()
        {
            var bootstrapper = new ServicesBootstrapper();
            var container = new Container();

            bootstrapper.Bootstrap(container);

            Assert.That(container.Resolve<IClock>(), Is.Not.Null);
        }
    }
}