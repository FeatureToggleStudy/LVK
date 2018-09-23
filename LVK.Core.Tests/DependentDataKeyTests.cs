using System;

using NUnit.Framework;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement

namespace LVK.Core.Tests
{
    [TestFixture]
    public class DependentDataKeyTests
    {
        [Test]
        public void Constructor_NullType_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DependentDataKey(null, "name"));
        }
        
        [Test]
        public void Constructor_NullName_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DependentDataKey(typeof(int), null));
        }

        [Test]
        [TestCase(typeof(int))]
        [TestCase(typeof(string))]
        public void Type_WithTestCases_ContainsTheCorrectValue(Type type)
        {
            var key = new DependentDataKey(type, "name");

            Assert.That(key.Type, Is.EqualTo(type));
        }
        
        [Test]
        [TestCase("name1")]
        [TestCase("name2")]
        public void Name_WithTestCases_ContainsTheCorrectValue(string name)
        {
            var key = new DependentDataKey(typeof(int), name);

            Assert.That(key.Name, Is.EqualTo(name));
        }
        
        [Test]
        public void Comparison_WithDifferentNames_AreNotEqual()
        {
            var key1 = new DependentDataKey(typeof(int), "name1");
            var key2 = new DependentDataKey(typeof(int), "name2");

            Assert.That(key1.GetHashCode(), Is.Not.EqualTo(key2.GetHashCode()));
            Assert.That(key1.Equals(key2), Is.False);
            Assert.That(key1.Equals((object)key2), Is.False);
        }
        
        [Test]
        public void Comparison_WithDifferentType_AreNotEqual()
        {
            var key1 = new DependentDataKey(typeof(int), "name");
            var key2 = new DependentDataKey(typeof(string), "name");

            Assert.That(key1.GetHashCode(), Is.Not.EqualTo(key2.GetHashCode()));
            Assert.That(key1.Equals(key2), Is.False);
            Assert.That(key1.Equals((object)key2), Is.False);
        }
        
        [Test]
        public void Comparison_WithSameTypeAndName_IsEqual()
        {
            var key1 = new DependentDataKey(typeof(int), "name");
            var key2 = new DependentDataKey(typeof(int), "name");

            Assert.That(key1.GetHashCode(), Is.EqualTo(key2.GetHashCode()));
            Assert.That(key1.Equals(key2), Is.True);
            Assert.That(key1.Equals((object)key2), Is.True);
        }
    }
}