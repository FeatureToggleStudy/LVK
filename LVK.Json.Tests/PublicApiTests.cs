using LVK.Tests.Framework;

using NUnit.Framework;

namespace LVK.Json.Tests
{
    [TestFixture]
    public class PublicApiTests : PublicApiTestsBase<ServicesBootstrapper>
    {
        // public static IEnumerable<TestCaseData> PublicTypes() => GetPublicTypesOfAssembly(typeof(JsonBuilder));
        //
        // [Test]
        // [TestCaseSource(nameof(PublicTypes))]
        // public void PublicType_IsTaggedWithPublicApi(Type publicType) => VerifyPublicApi(publicType);
    }
}