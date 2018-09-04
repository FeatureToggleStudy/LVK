using DryIoc;

using LVK.DryIoc;

using NUnit.Framework;

namespace LVK.Conversion.Tests
{
    [TestFixture]
    public class ValueConverterTests
    {
        [Test]
        public void Convert_SmokeTests()
        {
            var container = new Container();
            container.Bootstrap<ServicesBootstrapper>();

            var valueConverter = container.Resolve<IValueConverter>();

            Assert.That(valueConverter.Convert<int, uint>(10), Is.EqualTo(10));
            Assert.That(valueConverter.Convert<uint, int>(10), Is.EqualTo(10));
            Assert.That(valueConverter.Convert<int, string>(10), Is.EqualTo("10"));
        }
    }
}