using System;

using DryIoc;

using LVK.DryIoc;

using NodaTime;

using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.Core.Services.Tests
{
    [TestFixture]
    public class ServicesRegistrantTests
    {
        [Test]
        public void Register_NullContainer_ThrowsArgumentNullException()
        {
            var registrant = new ServicesRegistrant();
            Assert.Throws<ArgumentNullException>(() => registrant.Register((IContainer)null));
        }

        [Test]
        public void Register_NullContainerBuilder_ThrowsArgumentNullException()
        {
            var registrant = new ServicesRegistrant();
            Assert.Throws<ArgumentNullException>(() => registrant.Register((IContainerBuilder)null));
        }

        [Test]
        public void Register_WithContainer_RegistersClock()
        {
            var registrant = new ServicesRegistrant();
            var container = new Container();

            registrant.Register(container);

            Assert.That(container.Resolve<IClock>(), Is.Not.Null);
        }
    }
}