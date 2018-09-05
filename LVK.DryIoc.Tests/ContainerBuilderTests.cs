using DryIoc;

using NUnit.Framework;

namespace LVK.DryIoc.Tests
{
    [TestFixture]
    public class ContainerBuilderTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void RegisterRegistrant_SameRegistrantRegisteredTwice_OnlyCallsRegisterContainerBuilderOnRegistrantOnce()
        {
            var builder = new ContainerBuilder();
            
            builder.Register<DummyRegistrant>();
            builder.Register<DummyRegistrant>();

            Assert.That(DummyRegistrant.RegisterContainerBuilderCounter, Is.EqualTo(1));
        }

        [Test]
        public void RegisterRegistrant_SameRegistrantRegisteredTwice_OnlyCallsRegisterContainerOnRegistrantOnce()
        {
            var builder = new ContainerBuilder();
            
            builder.Register<DummyRegistrant>();
            builder.Register<DummyRegistrant>();

            builder.Build();

            Assert.That(DummyRegistrant.RegisterContainerCounter, Is.EqualTo(1));
        }
    }

    public class DummyRegistrant : IServicesRegistrant
    {
        public void Register(IContainerBuilder containerBuilder)
        {
            RegisterContainerBuilderCounter++;
        }

        public void Register(IContainer container)
        {
            RegisterContainerCounter++;
        }

        public static void Reset() => (RegisterContainerCounter, RegisterContainerBuilderCounter) = (0, 0);

        public static int RegisterContainerBuilderCounter { get; private set; }
        public static int RegisterContainerCounter { get; private set; }
    }
}