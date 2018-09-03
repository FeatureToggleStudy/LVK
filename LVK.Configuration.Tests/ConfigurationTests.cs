using System;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement

namespace LVK.Configuration.Tests
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void Constructor_NullRoot_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Configuration(null));
        }

        [Test]
        public void Value_OfElementOfTheWrongType_ReturnsDefaultValue()
        {
            JObject obj = JObject.Parse("{ }");

            var configuration = new Configuration(obj);

            var value = configuration.Value<string>();

            Assert.That(value, Is.Null);
        }

        [Test]
        public void Indexer_NullPath_ThrowsArgumentNullException()
        {
            JObject obj = JObject.Parse("{ }");

            var configuration = new Configuration(obj);

            Assert.Throws<ArgumentNullException>(() => GC.KeepAlive(configuration[(string)null]));
        }

        [Test]
        public void Indexer_NullPathArray_ThrowsArgumentNullException()
        {
            JObject obj = JObject.Parse("{ }");

            var configuration = new Configuration(obj);

            Assert.Throws<ArgumentNullException>(() => GC.KeepAlive(configuration[(string[])null]));
        }
        
        [Test]
        public void Indexer_PathToNonExistentValue_ReturnsConfigurationObject()
        {
            JObject obj = JObject.Parse("{ }");

            var configuration = new Configuration(obj);
            IConfiguration subConfiguration = configuration["path/to/non/existent/section"];

            Assert.That(subConfiguration, Is.Not.Null);
        }

        [Test]
        public void Indexer_PathToNonExistentValueThroughArray_ReturnsConfigurationObject()
        {
            JObject obj = JObject.Parse("{ }");

            var configuration = new Configuration(obj);
            IConfiguration subConfiguration = configuration[new[] { "path", "to", "non", "existent", "section" }];

            Assert.That(subConfiguration, Is.Not.Null);
        }
        
        [Test]
        public void Indexer_PathToExistingValue_ReturnsConfigurationObjectThatContainsThatValue()
        {
            JObject obj = JObject.Parse("{ \"Sub\": { \"Value\": 42 } }");

            var configuration = new Configuration(obj);
            var value = configuration["Sub/Value"].Value<int>();

            Assert.That(value, Is.EqualTo(42));
        }

        [Test]
        public void Indexer_PathToExistingValueThroughArray_ReturnsConfigurationObjectThatContainsThatValue()
        {
            JObject obj = JObject.Parse("{ \"Sub\": { \"Value\": 42 } }");

            var configuration = new Configuration(obj);
            var value = configuration[new[] { "Sub", "Value" }].Value<int>();

            Assert.That(value, Is.EqualTo(42));
        }
    }
}