using System;
using System.Collections.Generic;

using LVK.Tests.Framework;

using NUnit.Framework;

namespace LVK.AppCore.Console.Tests
{
    [TestFixture]
    public class PublicApiTests : PublicApiTestsBase
    {
        public static IEnumerable<TestCaseData> PublicTypes() => GetPublicTypesOfAssembly(typeof(Logging.ServicesBootstrapper));

        [Test]
        [TestCaseSource(nameof(PublicTypes))]
        public void PublicType_IsTaggedWithPublicApi(Type publicType) => VerifyPublicApi(publicType);
    }
}