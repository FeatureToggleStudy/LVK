using System;

using NUnit.Framework;

namespace LVK.Core.Tests
{
    [TestFixture]
    public class ActionDisposableTests
    {
        [Test]
        public void Constructor_NullInitAction_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => new ActionDisposable(null, () =>
            {
            }));
        }

        [Test]
        public void Constructor_NullDisposeAction_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionDisposable(null, null));
        }

        [Test]
        public void ConstructorWithOnlyDisposeAction_NullDisposeAction_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ActionDisposable(null));
        }
    }
}