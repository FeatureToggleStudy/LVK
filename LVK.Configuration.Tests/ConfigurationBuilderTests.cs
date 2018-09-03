using System;
using System.Text;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.Configuration.Tests
{
    [TestFixture]
    public class ConfigurationBuilderTests
    {
        [Test]
        public void AddJsonFile_NullFilename_ThrowsArgumentNullException()
        {
            var builder = new ConfigurationBuilder();
            Assert.Throws<ArgumentNullException>(() => builder.AddJsonFile(null, Encoding.Default, true));
        }

        [Test]
        public void AddJsonFile_NullEncoding_DoesNotThrowArgumentNullException()
        {
            var builder = new ConfigurationBuilder();
            Assert.DoesNotThrow(() => builder.AddJsonFile("appsettings.json", null, true));
        }
        
        [Test]
        public void AddJson_NullJson_ThrowsArgumentNullException()
        {
            var builder = new ConfigurationBuilder();
            Assert.Throws<ArgumentNullException>(() => builder.AddJson(null));
        }

        [Test]
        public void AddJson_OverridesDictionary_AddsToDictionary()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJson("{ \"a\": { \"v1\": 42 } }");
            builder.AddJson("{ \"a\": { \"v2\": 17 } }");

            IConfiguration configuration = builder.Build();

            var v1 = configuration["a/v1"].Value<int>();
            var v2 = configuration["a/v2"].Value<int>();

            Assert.That(v1, Is.EqualTo(42));
            Assert.That(v2, Is.EqualTo(17));
        }

        [Test]
        public void AddJson_OverridesDictionaryWithSameKeys_ReplacesKeysInDictionary()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJson("{ \"a\": { \"v1\": 42 } }");
            builder.AddJson("{ \"a\": { \"v1\": 17 } }");

            IConfiguration configuration = builder.Build();

            var v1 = configuration["a/v1"].Value<int>();

            Assert.That(v1, Is.EqualTo(17));
        }
        
        [Test]
        public void AddJson_OverridesArray_ReplacesArray()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJson("{ \"a\": [ 10, 20 ] }");
            builder.AddJson("{ \"a\": [ 15 ] }");

            IConfiguration configuration = builder.Build();

            var a = configuration["a"].Value<int[]>();

            CollectionAssert.AreEqual(new[] { 15 }, a);
        }

        [Test]
        public void AddCommandLine_NullArgs_ThrowsArgumentNullException()
        {
            var builder = new ConfigurationBuilder();

            Assert.Throws<ArgumentNullException>(() => builder.AddCommandLine(null));
        }

        [Test]
        public void AddCommandLine_ExistingKey_ReplacesKeyValue()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJson("{ \"a\": [ 10, 20 ] }");

            builder.AddCommandLine(new[] { "--a=[15, 42]" });

            IConfiguration configuration = builder.Build();
            var a = configuration["a"].Value<int[]>();
            CollectionAssert.AreEqual(new[] { 15, 42 }, a);
        }

        [Test]
        public void AddCommandLine_NewKeyIntoDictionary_AddsToDictionary()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJson("{ \"a\": { \"v1\": 42 } }");

            builder.AddCommandLine(new[] { "--a/v2=17" });

            IConfiguration configuration = builder.Build();
            var v1 = configuration["a/v1"].Value<int>();
            var v2 = configuration["a/v2"].Value<int>();

            Assert.That(v1, Is.EqualTo(42));
            Assert.That(v2, Is.EqualTo(17));
        }

        [Test]
        public void AddCommandLine_ValueThatIsNotJson_IsAddedAsString()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJson("{ \"a\": { \"v1\": 42 } }");

            builder.AddCommandLine(new[] { "--a/v1=x15" });

            IConfiguration configuration = builder.Build();
            var v1 = configuration["a/v1"].Value<string>();

            Assert.That(v1, Is.EqualTo("x15"));
        }

        [Test]
        public void AddEnvironmentVariables_NullPrefix_ThrowsArgumentNullException()
        {
            var builder = new ConfigurationBuilder();
            
            Assert.Throws<ArgumentNullException>(() => builder.AddEnvironmentVariables(null));
        }

        [Test]
        public void AddEnvironmentVariables_NoVariablesThatMatch_DoesNotAddAnythingToConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables("DUMMY_VARIABLE_PREFIX_!\"'");

            IConfiguration configuration = builder.Build();
            var obj = configuration.Value<JObject>();

            Assert.That(obj.First, Is.Null);
        }

        [Test]
        public void AddEnvironmentVariables_OneVariableThatMatches_AddsThatValueToTheConfiguration()
        {
            var builder = new ConfigurationBuilder();
            Environment.SetEnvironmentVariable("DUMMY_VARIABLE_VALUE", "[10, 20]");
            builder.AddEnvironmentVariables("DUMMY_VARIABLE_");

            IConfiguration configuration = builder.Build();

            var value = configuration["VALUE"].Value<int[]>();

            CollectionAssert.AreEqual(new[] { 10, 20 }, value);
        }
    }
}