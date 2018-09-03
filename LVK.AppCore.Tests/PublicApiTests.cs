using System;
using System.Collections.Generic;

using LVK.Tests.Framework;

using NUnit.Framework;

namespace LVK.AppCore.Tests
{
    [TestFixture]
    public class PublicApiTests : PublicApiTestsBase
    {
        public static IEnumerable<TestCaseData> PublicTypes() => GetPublicTypesOfAssembly(typeof(Core.Services.ServicesBootstrapper));

        [Test]
        [TestCaseSource(nameof(PublicTypes))]
        public void PublicType_IsTaggedWithPublicApi(Type publicType) => VerifyPublicApi(publicType);
    }
}