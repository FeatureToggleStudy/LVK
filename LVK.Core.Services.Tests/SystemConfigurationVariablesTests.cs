using System.Collections.Generic;
using System.Linq;

using DryIoc;

using LVK.DryIoc;

using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.Core.Services.Tests
{
    [TestFixture]
    public class SystemConfigurationVariablesTests
    {
        [Test]
        [TestCase("sys.ProcessId")]
        [TestCase("sys.ProcessName")]
        [TestCase("sys.MachineName")]
        [TestCase("sys.UserName")]
        [TestCase("sys.UserDomainName")]
        public void TryGetValue_ExistingVariables_ReturnsSuccessTrue(string key)
        {
            var container = ContainerFactory.Create();
            container.Bootstrap<ServicesBootstrapper>();
            var scv = container.Resolve<IEnumerable<IConfigurationVariables>>().OfType<SystemConfigurationVariables>().First();

            var (success, _) = scv.TryGetValue(key);

            Assert.That(success, Is.True);
        }
    }
}