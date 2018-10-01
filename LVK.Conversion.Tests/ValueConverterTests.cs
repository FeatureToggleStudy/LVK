using DryIoc;

using LVK.DryIoc;

using NUnit.Framework;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Conversion.Tests
{
    [TestFixture]
    public class ValueConverterTests
    {
        [Test]
        public void Convert_SmokeTests()
        {
            var container = ContainerFactory.Create().Bootstrap<ServicesBootstrapper>();

            var valueConverter = container.Resolve<IValueConverter>();
            assume(valueConverter != null);

            Assert.That(valueConverter.Convert<int, uint>(10), Is.EqualTo(10));
            Assert.That(valueConverter.Convert<uint, int>(10), Is.EqualTo(10));
            Assert.That(valueConverter.Convert<int, string>(10), Is.EqualTo("10"));
            Assert.That(valueConverter.Convert<int, int>(10), Is.EqualTo(10));
        }
    }
}