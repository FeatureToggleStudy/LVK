using System.Linq;

using LVK.Core.Services;

using NSubstitute;

using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.Configuration.Tests
{
    [TestFixture]
    public class VariableConfigurationDecoderTests
    {
        [Test]
        [TestCase("this is a test")]
        [TestCase("This is a test with a dollar sign $ dollar")]
        [TestCase("This is a test with an invalid key ${test}")]
        public void Decode_StringWithoutVariables_ReturnsStringUnmodified(string input)
        {
            var vcd = new VariableConfigurationDecoder(Enumerable.Empty<IConfigurationVariables>());

            var output = vcd.Decode(input);

            Assert.That(output, Is.EqualTo(input));
        }

        [Test]
        [TestCase("${sys.Variable1}", "Value1")]
        [TestCase("${sys.Variable2}", "Value2")]
        [TestCase("${sys.Variable1} - ${sys.Variable2}", "Value1 - Value2")]
        public void Decode_StringWithVariables_ReturnsDecodedString(string input, string expected)
        {
            var cv1 = Substitute.For<IConfigurationVariables>();
            cv1.Prefix.Returns("sys");
            cv1.TryGetValue("sys.Variable1").Returns((true, "Value1"));
            cv1.TryGetValue("sys.Variable2").Returns((true, "Value2"));
            var vcd = new VariableConfigurationDecoder(new[] { cv1 });

            var output = vcd.Decode(input);

            Assert.That(output, Is.EqualTo(expected));
        }
    }
}