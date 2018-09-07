using System;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.Core.Services.Tests
{
    [TestFixture]
    public class BusTests
    {
        [Test]
        public void Publish_NullMessage_ThrowsArgumentNullException()
        {
            var bus = new Bus();
            Assert.Throws<ArgumentNullException>(() => bus.Publish<string>(null));
        }

        [Test]
        public void Publish_ActualMessageButNoSubscribers_DoesntThrownException()
        {
            var bus = new Bus();
            
            Assert.DoesNotThrow(() => bus.Publish("test"));
        }

        [Test]
        public void Publish_OneSubscriber_CallsThatSubscriber()
        {
            var bus = new Bus();
            string message = null;
            bus.Subscribe<string>(m => message = m);

            bus.Publish("Test");

            Assert.That(message, Is.EqualTo("Test"));
        }

        [Test]
        public void Publish_OneSubscriberThatHasBeenUnsubscribed_DoesNotCallThatSubscriber()
        {
            var bus = new Bus();
            string message = null;
            bus.Subscribe<string>(m => message = m).Dispose();

            bus.Publish("Test");

            Assert.That(message, Is.Null);
        }
    }
}