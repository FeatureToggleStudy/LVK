using System;
using System.IO;
using System.Text;

using LVK.Configuration.Layers.JsonFile;

using Newtonsoft.Json;

using NUnit.Framework;

// ReSharper disable AssignmentIsFullyDiscarded
// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement

namespace LVK.Configuration.Tests.Layers.JsonFile
{
    [TestFixture]
    public class JsonFileConfigurationLayerTests
    {
        private string _Filename;

        [SetUp]
        public void SetUp()
        {
            _Filename = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");
            File.WriteAllText(_Filename, "{}", Encoding.UTF8);
        }

        public void TearDown()
        {
            if (File.Exists(_Filename))
                File.Delete(_Filename);
        }

        [Test]
        public void Constructor_NullFilename_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonFileConfigurationLayer(null, Encoding.UTF8, false));
        }

        [Test]
        public void Constructor_NullEncoding_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new JsonFileConfigurationLayer("test.json", null, false));
        }

        [Test]
        public void Configuration_WhenConfigurationFileDoesNotExistAndFileIsNotOptional_ThrowsInvalidOperationException()
        {
            File.Delete(_Filename);

            var layer = new JsonFileConfigurationLayer(_Filename, Encoding.UTF8, false);

            Assert.Throws<InvalidOperationException>(() => _ = layer.Configuration);
        }

        [Test]
        public void Configuration_WhenConfigurationFileDoesNotExistAndFileIsOptional_ReturnsNull()
        {
            File.Delete(_Filename);

            var layer = new JsonFileConfigurationLayer(_Filename, Encoding.UTF8, true);
            var configuration = layer.Configuration;

            Assert.That(configuration, Is.Null);
        }
        
        [Test]
        public void Configuration_WhenConfigurationFileContainsValues_ContainsThoseValues()
        {
            var co = new ConfigurationObject { Value1 = "Test 123" };
            File.WriteAllText(_Filename, JsonConvert.SerializeObject(co));

            var layer = new JsonFileConfigurationLayer(_Filename, Encoding.UTF8, false);

            var configuration = layer.Configuration;

            var output = configuration.ToObject<ConfigurationObject>();

            Assert.That(JsonConvert.SerializeObject(output), Is.EqualTo(JsonConvert.SerializeObject(co)));
        }

        [Test]
        public void Configuration_WhenConfigurationFileFirstDoesNotExistAndThenIsCreated_FirstReturnsNullThenReturnsConfiguration()
        {
            var layer = new JsonFileConfigurationLayer(_Filename, Encoding.UTF8, true);
            
            File.Delete(_Filename);
            var configuration1 = layer.Configuration;
            
            var co = new ConfigurationObject { Value1 = "Test 123" };
            File.WriteAllText(_Filename, JsonConvert.SerializeObject(co));
            var configuration2 = layer.Configuration;

            File.Delete(_Filename);
            var configuration3 = layer.Configuration;

            Assert.That(configuration1, Is.Null);
            Assert.That(JsonConvert.SerializeObject(configuration2), Is.EqualTo(JsonConvert.SerializeObject(co)));
            Assert.That(configuration3, Is.Null);
        }

        [Test]
        public void Configuration_WhenConfigurationFileIsModified_ReturnsNewValues()
        {
            var layer = new JsonFileConfigurationLayer(_Filename, Encoding.UTF8, true);
            
            var input1 = new ConfigurationObject { Value1 = "Test 123" };
            File.WriteAllText(_Filename, JsonConvert.SerializeObject(input1));
            var configuration1 = layer.Configuration;

            var input2 = new ConfigurationObject { Value1 = "Test 456" };
            File.WriteAllText(_Filename, JsonConvert.SerializeObject(input2));
            var configuration2 = layer.Configuration;
            
            Assert.That(JsonConvert.SerializeObject(configuration1), Is.EqualTo(JsonConvert.SerializeObject(input1)));
            Assert.That(JsonConvert.SerializeObject(configuration2), Is.EqualTo(JsonConvert.SerializeObject(input2)));
            Assert.That(JsonConvert.SerializeObject(configuration1), Is.Not.EqualTo(JsonConvert.SerializeObject(configuration2)));
        }
    }
}