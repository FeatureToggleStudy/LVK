using System;

using DryIoc;

using LVK.DryIoc;

using NodaTime;

using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.NodaTime.Tests
{
    [TestFixture]
    public class ServicesBootstrapperTests
    {
        [Test]
        public void Register_NullContainer_ThrowsArgumentNullException()
        {
            var bootstrapper = new ServicesBootstrapper();
            Assert.Throws<ArgumentNullException>(() => bootstrapper.Bootstrap(null));
        }

        [Test]
        public void Register_WithContainer_RegistersClock()
        {
            var bootstrapper = new ServicesBootstrapper();
            var container = ContainerFactory.Create();

            bootstrapper.Bootstrap(container);

            Assert.That(container.Resolve<IClock>(), Is.Not.Null);
        }
    }
}