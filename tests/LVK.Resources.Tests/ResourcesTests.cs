using System;
using System.Resources;

using DryIoc;

using LVK.DryIoc;
using LVK.Json;
using LVK.Logging;

using NSubstitute;

using NUnit.Framework;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LVK.Resources.Tests
{
    [TestFixture]
    public class ResourcesTests
    {
        [Test]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Resources(null, Substitute.For<IJsonSerializerFactory>(), GetType().Assembly, "name"));
        }

        [Test]
        public void Constructor_NullJsonSerializerFactory_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Resources(Substitute.For<ILogger>(), null, GetType().Assembly, "name"));
        }

        [Test]
        public void Constructor_NullAssembly_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Resources(Substitute.For<ILogger>(),Substitute.For<IJsonSerializerFactory>(), null, "name"));
        }

        [Test]
        public void Constructor_NullNamePrefix_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Resources(Substitute.For<ILogger>(),Substitute.For<IJsonSerializerFactory>(), GetType().Assembly, null));
        }

        [Test]
        public void DeserializeJson_WhenResourceExists_ReturnsDeserializedObject()
        {
            IContainer container = ContainerFactory.Bootstrap<ServicesBootstrapper>();
            var resourcesFactory = container.Resolve<IResourcesFactory>();
            var resources = resourcesFactory.GetResources<ResourcesTests>();

            var obj = resources.DeserializeJson<TestObject>("Resources.ExistingFile.json");

            Assert.That(obj.Value, Is.EqualTo(42));
        }

        [Test]
        public void DeserializeJson_WhenResourceDoesNotExist_ThrowsMissingManifestResourceException()
        {
            IContainer container = ContainerFactory.Bootstrap<ServicesBootstrapper>();
            var resourcesFactory = container.Resolve<IResourcesFactory>();
            var resources = resourcesFactory.GetResources<ResourcesTests>();

            Assert.Throws<MissingManifestResourceException>(() => resources.DeserializeJson<TestObject>("Resources.MissingFile.json"));
        }
    }

    public class TestObject
    {
        public int Value { get; set; }
    }
}