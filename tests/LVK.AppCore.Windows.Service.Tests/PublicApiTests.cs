using LVK.Tests.Framework;

using NUnit.Framework;

namespace LVK.AppCore.Windows.Service.Tests
{
    [TestFixture]
    internal class PublicApiTests : PublicApiTestsBase<ServicesBootstrapper<DummyServicesBootstrapper>>
    {
    }
}