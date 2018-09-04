using System;
using System.Collections.Generic;

using LVK.Tests.Framework;

using NUnit.Framework;

namespace LVK.DryIoc.Tests
{
    [TestFixture]
    public class PublicApiTests : PublicApiTestsBase
    {
        public static IEnumerable<TestCaseData> PublicTypes() => GetPublicTypesOfAssembly(typeof(ContainerExtensions));

        [Test]
        [TestCaseSource(nameof(PublicTypes))]
        public void PublicType_IsTaggedWithPublicApi(Type publicType) => VerifyPublicApi(publicType);
    }
}