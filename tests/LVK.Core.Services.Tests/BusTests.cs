using System;
using System.Threading.Tasks;

using DryIoc;

using LVK.DryIoc;

using NSubstitute;

using NUnit.Framework;

#pragma warning disable 4014
// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.Core.Services.Tests
{
    public class EmptyServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
        }
    }

    [TestFixture]
    public class BusTests
    {
        [Test]
        public void Publish_NullMessage_ThrowsArgumentNullException()
        {
            var container = ContainerFactory.Bootstrap<EmptyServicesBootstrapper>();
            var bus = new Bus(container);
            Assert.Throws<ArgumentNullException>(() => bus.PublishAsync((string)null));
        }

        [Test]
        public void Publish_NullGetMessage_ThrowsArgumentNullException()
        {
            var container = ContainerFactory.Bootstrap<EmptyServicesBootstrapper>();
            var bus = new Bus(container);
            Assert.Throws<ArgumentNullException>(() => bus.PublishAsync((Func<string>)null));
        }

        [Test]
        public void Publish_NullMessageReturnedFromGetMessageButNoSubscribers_DoesNotThrow()
        {
            var container = ContainerFactory.Bootstrap<EmptyServicesBootstrapper>();
            var bus = new Bus(container);
            Assert.DoesNotThrow(() => bus.PublishAsync(() => (string)null));
        }

        [Test]
        public void Publish_NullMessageReturnedFromGetMessage_ThrowsInvalidOperationException()
        {
            var container = ContainerFactory.Bootstrap<EmptyServicesBootstrapper>();
            var bus = new Bus(container);
            var subscriber = Substitute.For<ISubscriber<string>>();
            bus.Subscribe(subscriber);

            Assert.Throws<InvalidOperationException>(() => bus.PublishAsync(() => (string)null));
        }
        
        [Test]
        public void Publish_ActualMessageButNoSubscribers_DoesntThrownException()
        {
            var container = ContainerFactory.Bootstrap<EmptyServicesBootstrapper>();
            var bus = new Bus(container);
            
            Assert.DoesNotThrow(() => bus.PublishAsync("test"));
        }

        [Test]
        public async Task Publish_OneSubscriber_CallsThatSubscriber()
        {
            var container = ContainerFactory.Bootstrap<EmptyServicesBootstrapper>();
            var bus = new Bus(container);
            string message = null;
            bus.Subscribe<string>(m => message = m);

            await bus.PublishAsync("Test");

            Assert.That(message, Is.EqualTo("Test"));
        }

        [Test]
        public async Task Publish_OneSubscriberThatHasBeenUnsubscribed_DoesNotCallThatSubscriber()
        {
            var container = ContainerFactory.Bootstrap<EmptyServicesBootstrapper>();
            var bus = new Bus(container);
            string message = null;
            bus.Subscribe<string>(m => message = m).Dispose();

            await bus.PublishAsync("Test");

            Assert.That(message, Is.Null);
        }

        [Test]
        public async Task Publish_SubscriberThatHasNotBeenCollected_CallsThatSubscriber()
        {
            var container = ContainerFactory.Bootstrap<EmptyServicesBootstrapper>();
            var bus = new Bus(container);
            Subscriber.LastMessage = null;
            var subscriber = new Subscriber();
            bus.Subscribe(subscriber);
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            await bus.PublishAsync("Test");

            Assert.That(Subscriber.LastMessage, Is.EqualTo("Test"));
            GC.KeepAlive(subscriber);
        }

        [Test]
        public async Task Publish_SubscriberThatHasNotBeenCollectedThroughSubscription_CallsThatSubscriber()
        {
            var container = ContainerFactory.Bootstrap<EmptyServicesBootstrapper>();
            var bus = new Bus(container);
            Subscriber.LastMessage = null;
            var subscription = SubscribeAndReturnSubscription(bus);
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            await bus.PublishAsync("Test");

            Assert.That(Subscriber.LastMessage, Is.EqualTo("Test"));
            GC.KeepAlive(subscription);
        }
        
        [Test]
        public async Task Publish_SubscriberThatHasBeenCollected_DoesNotCallThatSubscriber()
        {
            var container = ContainerFactory.Bootstrap<EmptyServicesBootstrapper>();
            var bus = new Bus(container);
            Subscriber.LastMessage = null;
            Subscribe(bus);
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            await bus.PublishAsync("Test");

            Assert.That(Subscriber.LastMessage, Is.Null);
        }

        [Test]
        public async Task Publish_GetMessageWhenNoSubscribers_IsNotCalled()
        {
            var container = ContainerFactory.Bootstrap<EmptyServicesBootstrapper>();
            var bus = new Bus(container);
            var getMessage = Substitute.For<Func<string>>();

            await bus.PublishAsync(getMessage);

            getMessage.DidNotReceive().Invoke();
        }

        [Test]
        public async Task Publish_GetMessageWhenSubscribers_IsCalled()
        {
            var container = ContainerFactory.Bootstrap<EmptyServicesBootstrapper>();
            var bus = new Bus(container);
            var getMessage = Substitute.For<Func<string>>();
            var subscriber = Substitute.For<ISubscriber<string>>();
            getMessage.Invoke().Returns("Message");
            bus.Subscribe(subscriber);

            await bus.PublishAsync(getMessage);

            getMessage.Received().Invoke();
            subscriber.Received().Notify("Message");
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