using System;
using System.Collections.Generic;

using LVK.Tests.Framework;

using NUnit.Framework;

namespace LVK.NodaTime.Tests
{
    [TestFixture]
    public class ServiceBootstrapperTests : ServiceBootstrapperTestsBase<ServicesBootstrapper>
    {
        public static IEnumerable<Type> BootstrapperTypes() => ServiceBootstrappersInReferencedAssemblies();

        [Test]
        [TestCaseSource(nameof(BootstrapperTypes))]
        public void Bootstraper_InDependendentAssembly_IsBootstrapped(Type servicesBootstrapperType) => VerifyDependency(servicesBootstrapperType);
    }
}