using System;

using JetBrains.Annotations;

using NUnit.Framework;

namespace LVK.Data.Caching.Tests
{
    [TestFixture]
    public class WeakCacheTests
    {
        [Test]
        public void TryGetValue_ValueDoesNotExist_ReturnsSuccessFalse()
        {
            var cache = new WeakCache<string, string>();
            
            var (success, _) = cache.TryGetValue("Key");

            Assert.That(success, Is.False);
        }

        [Test]
        public void TryGetValue_ValueDoesNotExist_ReturnsValueDefault()
        {
            var cache = new WeakCache<string, string>();
            
            var (_, value) = cache.TryGetValue("Key");

            Assert.That(value, Is.Null);
        }
        
        [Test]
        public void TryGetValue_ValueExists_ReturnsSuccessTrue()
        {
            var cache = new WeakCache<string, string>();
            cache.GetOrAddValue("Key", key => "Value");
            
            var (success, _) = cache.TryGetValue("Key");
            
            Assert.That(success, Is.True);
        }

        [Test]
        public void TryGetValue_ValueExists_ReturnsValue()
        {
            var cache = new WeakCache<string, string>();
            cache.GetOrAddValue("Key", key => "Value");
            
            var (_, value) = cache.TryGetValue("Key");

            Assert.That(value, Is.EqualTo("Value"));
        }

        [Test]
        public void TryGetValue_ValueHasBeenCleared_ReturnsSuccessFalse()
        {
            var cache = new WeakCache<string, object>();
            AddToCacheInDifferentMethodToAvoidDebugVariableLifetimeProblems(cache);
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            var (success, _) = cache.TryGetValue("Key");

            Assert.That(success, Is.False);
        }

        private void AddToCacheInDifferentMethodToAvoidDebugVariableLifetimeProblems([NotNull] WeakCache<string,object> cache)
        {
            cache.GetOrAddValue("Key", key => new object());

            var (success, _) = cache.TryGetValue("Key");

            Assert.That(success, Is.True);
        }
    }
}