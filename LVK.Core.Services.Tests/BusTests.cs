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

        [Test]
        public void Publish_SubscriberThatHasNotBeenCollected_CallsThatSubscriber()
        {
            var bus = new Bus();
            Subscriber.LastMessage = null;
            var subscriber = new Subscriber();
            bus.Subscribe(subscriber);
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            bus.Publish("Test");

            Assert.That(Subscriber.LastMessage, Is.EqualTo("Test"));
            GC.KeepAlive(subscriber);
        }

        [Test]
        public void Publish_SubscriberThatHasNotBeenCollectedThroughSubscription_CallsThatSubscriber()
        {
            var bus = new Bus();
            Subscriber.LastMessage = null;
            var subscription = SubscribeAndReturnSubscription(bus);
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            bus.Publish("Test");

            Assert.That(Subscriber.LastMessage, Is.EqualTo("Test"));
            GC.KeepAlive(subscription);
        }
        
        [Test]
        public void Publish_SubscriberThatHasBeenCollected_DoesNotCallThatSubscriber()
        {
            var bus = new Bus();
            Subscriber.LastMessage = null;
            Subscribe(bus);
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            bus.Publish("Test");

            Assert.That(Subscriber.LastMessage, Is.Null);
        }

        private void Subscribe(Bus bus)
        {
            bus.Subscribe(new Subscriber());
        }

        private IDisposable SubscribeAndReturnSubscription(Bus bus)
        {
            return bus.Subscribe(new Subscriber());
        }
        
        private class Subscriber : ISubscriber<string>
        {
            public static string LastMessage;

            public void Notify(string message)
            {
                LastMessage = message;
            }
        }
    }
}