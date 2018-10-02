using System;
using System.Threading.Tasks;

using LVK.DryIoc;

using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.Core.Services.Tests
{
    [TestFixture]
    public class BusTests
    {
        [Test]
        public void Publish_NullMessage_ThrowsArgumentNullException()
        {
            var container = ContainerFactory.Create();
            var bus = new Bus(container);
            Assert.Throws<ArgumentNullException>(() => bus.Publish<string>(null));
        }

        [Test]
        public void Publish_ActualMessageButNoSubscribers_DoesntThrownException()
        {
            var container = ContainerFactory.Create();
            var bus = new Bus(container);
            
            Assert.DoesNotThrow(() => bus.Publish("test"));
        }

        [Test]
        public void Publish_OneSubscriber_CallsThatSubscriber()
        {
            var container = ContainerFactory.Create();
            var bus = new Bus(container);
            string message = null;
            bus.Subscribe<string>(m => message = m);

            bus.Publish("Test");

            Assert.That(message, Is.EqualTo("Test"));
        }

        [Test]
        public void Publish_OneSubscriberThatHasBeenUnsubscribed_DoesNotCallThatSubscriber()
        {
            var container = ContainerFactory.Create();
            var bus = new Bus(container);
            string message = null;
            bus.Subscribe<string>(m => message = m).Dispose();

            bus.Publish("Test");

            Assert.That(message, Is.Null);
        }

        [Test]
        public void Publish_SubscriberThatHasNotBeenCollected_CallsThatSubscriber()
        {
            var container = ContainerFactory.Create();
            var bus = new Bus(container);
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
            var container = ContainerFactory.Create();
            var bus = new Bus(container);
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
            var container = ContainerFactory.Create();
            var bus = new Bus(container);
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

            public Task Notify(string message)
            {
                LastMessage = message;
                return Task.CompletedTask;
            }
        }
    }
}