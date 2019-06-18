using System;

using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement
// ReSharper disable AssignNullToNotNullAttribute

namespace LVK.Performance.Counters.Tests
{
    [TestFixture]
    public class PerformanceCounterTests
    {
        [Test]
        public void Constructor_NullKey_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PerformanceCounter(null));
        }

        [Test]
        public void Value_AfterConstruction_IsZero()
        {
            PerformanceCounter counter = new PerformanceCounter("key");

            long output = counter.Value;
            
            Assert.That(output, Is.EqualTo(0));
        }

        [Test]
        public void Increment_AfterAlreadyHavingBeenCalledTwice_ReturnsThree()
        {
            PerformanceCounter counter = new PerformanceCounter("key");
            counter.Increment();
            counter.Increment();

            long output = counter.Increment();

            Assert.That(output, Is.EqualTo(3));
        }

        [Test]
        public void Value_AfterIncrementOnce_IsOne()
        {
            PerformanceCounter counter = new PerformanceCounter("key");
            counter.Increment();

            long output = counter.Value;
            
            Assert.That(output, Is.EqualTo(1));
        }

        [Test]
        public void Value_AfterIncrementTwice_IsTwo()
        {
            PerformanceCounter counter = new PerformanceCounter("key");
            counter.Increment();
            counter.Increment();
            
            long output = counter.Value;
            
            Assert.That(output, Is.EqualTo(2));
        }

        [Test]
        public void Value_AfterIncrementAndThenReset_IsZero()
        {
            PerformanceCounter counter = new PerformanceCounter("key");
            counter.Increment();

            counter.Reset();
            long output = counter.Value;
            
            Assert.That(output, Is.EqualTo(0));
        }
    }
}