using System;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.Core.Services.Tests
{
    [TestFixture]
    public class EnvironmentConfigurationVariablesTests
    {
        [Test]
        public void TryGetValue_KeyDoesNotExist_ReturnsSuccessFalse()
        {
            var ecv = new EnvironmentConfigurationVariables();

            Environment.SetEnvironmentVariable("NONEXISTANTKEY", string.Empty);
            var (success, _) = ecv.TryGetValue("env.NONEXISTANTKEY");

            Assert.That(success, Is.False);
        }

        [Test]
        public void TryGetValue_KeyDoesNotExist_ReturnsNullValue()
        {
            var ecv = new EnvironmentConfigurationVariables();

            Environment.SetEnvironmentVariable("NONEXISTANTKEY", string.Empty);
            var (_, value) = ecv.TryGetValue("env.NONEXISTANTKEY");

            Assert.That(value, Is.Null);
        }
        
        [Test]
        public void TryGetValue_KeyExists_ReturnsSuccessTrue()
        {
            var ecv = new EnvironmentConfigurationVariables();

            Environment.SetEnvironmentVariable("SOMEKEY", "Some value");
            var (success, _) = ecv.TryGetValue("env.SOMEKEY");

            Assert.That(success, Is.True);
        }

        [Test]
        public void TryGetValue_KeyExists_ReturnsValue()
        {
            var ecv = new EnvironmentConfigurationVariables();

            Environment.SetEnvironmentVariable("SOMEKEY", "Some value");
            var (_, value) = ecv.TryGetValue("env.SOMEKEY");

            Assert.That(value, Is.EqualTo("Some value"));
        }

        [Test]
        [TestCase("env:SOMEKEY")]
        [TestCase("SOMEKEY")]
        public void TryGetValue_IncorrectFormatOfKeyButVariableExists_ReturnsSuccessFalse(string key)
        {
            var ecv = new EnvironmentConfigurationVariables();

            Environment.SetEnvironmentVariable("SOMEKEY", "Some value");
            var (success, _) = ecv.TryGetValue(key);

            Assert.That(success, Is.False);
        }
    }
}