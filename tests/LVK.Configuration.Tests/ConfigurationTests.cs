using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NSubstitute;

using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement

namespace LVK.Configuration.Tests
{
    [TestFixture]
    public class ConfigurationTests
    {
        private IConfigurationProvider _ConfigurationProvider;
        private JsonSerializer _JsonSerializer;

        [SetUp]
        public void SetUp()
        {
            _ConfigurationProvider = Substitute.For<IConfigurationProvider>();
            _ConfigurationProvider.GetConfiguration().Returns(new JObject());

            _JsonSerializer = new JsonSerializer();
        }

        [Test]
        public void Constructor_NullConfigurationProvider_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RootConfiguration(null, string.Empty, _JsonSerializer));
        }

        [Test]
        public void Constructor_NullPath_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RootConfiguration(_ConfigurationProvider, null, _JsonSerializer));
        }

        [Test]
        public void ValueOrDefault_OfElementOfTheWrongType_ReturnsDefaultValue()
        {
            JObject obj = JObject.Parse("{ }");
            _ConfigurationProvider.GetConfiguration().Returns(obj);

            var configuration = new RootConfiguration(_ConfigurationProvider, string.Empty, _JsonSerializer);

            var value = configuration.Element<string>().ValueOrDefault();

            Assert.That(value, Is.Null);
        }

        [Test]
        public void Indexer_NullPath_ThrowsArgumentNullException()
        {
            JObject obj = JObject.Parse("{ }");
            _ConfigurationProvider.GetConfiguration().Returns(obj);

            var configuration = new RootConfiguration(_ConfigurationProvider, string.Empty, _JsonSerializer);

            Assert.Throws<ArgumentNullException>(() => GC.KeepAlive(configuration[(string)null]));
        }

        [Test]
        public void Indexer_NullPathArray_ThrowsArgumentNullException()
        {
            JObject obj = JObject.Parse("{ }");
            _ConfigurationProvider.GetConfiguration().Returns(obj);

            var configuration = new RootConfiguration(_ConfigurationProvider, string.Empty, _JsonSerializer);

            Assert.Throws<ArgumentNullException>(() => GC.KeepAlive(configuration[(string[])null]));
        }

        [Test]
        public void Indexer_PathToNonExistentValue_ReturnsConfigurationObject()
        {
            JObject obj = JObject.Parse("{ }");
            _ConfigurationProvider.GetConfiguration().Returns(obj);

            var configuration = new RootConfiguration(_ConfigurationProvider, string.Empty, _JsonSerializer);
            IConfiguration subConfiguration = configuration["path/to/non/existent/section"];

            Assert.That(subConfiguration, Is.Not.Null);
        }

        [Test]
        public void Indexer_PathToNonExistentValueThroughArray_ReturnsConfigurationObject()
        {
            JObject obj = JObject.Parse("{ }");
            _ConfigurationProvider.GetConfiguration().Returns(obj);

            var configuration = new RootConfiguration(_ConfigurationProvider, string.Empty, _JsonSerializer);
            IConfiguration subConfiguration = configuration[new[] { "path", "to", "non", "existent", "section" }];

            Assert.That(subConfiguration, Is.Not.Null);
        }

        [Test]
        public void Indexer_PathToExistingValue_ReturnsConfigurationObjectThatContainsThatValue()
        {
            JObject obj = JObject.Parse("{ \"Sub\": { \"Value\": 42 } }");
            _ConfigurationProvider.GetConfiguration().Returns(obj);

            var configuration = new RootConfiguration(_ConfigurationProvider, string.Empty, _JsonSerializer);
            var value = configuration["Sub/Value"].Element<int>().Value();

            Assert.That(value, Is.EqualTo(42));
        }

        [Test]
        public void Indexer_PathToExistingValueThroughArray_ReturnsConfigurationObjectThatContainsThatValue()
        {
            JObject obj = JObject.Parse("{ \"Sub\": { \"Value\": 42 } }");
            _ConfigurationProvider.GetConfiguration().Returns(obj);

            var configuration = new RootConfiguration(_ConfigurationProvider, string.Empty, _JsonSerializer);
            var value = configuration[new[] { "Sub", "Value" }].Element<int>().Value();

            Assert.That(value, Is.EqualTo(42));
        }
    }
}