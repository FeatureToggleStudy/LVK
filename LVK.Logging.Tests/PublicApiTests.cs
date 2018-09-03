using System;
using System.Collections.Generic;

using LVK.Tests.Framework;

using NUnit.Framework;

namespace LVK.Logging.Tests
{
    [TestFixture]
    public class PublicApiTests : PublicApiTestsBase
    {
        public static IEnumerable<TestCaseData> PublicTypes() => GetPublicTypesOfAssembly(typeof(TextLogFormatter));

        [Test]
        [TestCaseSource(nameof(PublicTypes))]
        public void PublicType_IsTaggedWithPublicApi(Type publicType) => VerifyPublicApi(publicType);
    }
}