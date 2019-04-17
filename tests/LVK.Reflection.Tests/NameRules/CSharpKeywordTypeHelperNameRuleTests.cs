using System;

using LVK.Reflection.NameRules;

using NSubstitute;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException

namespace LVK.Reflection.Tests.NameRules
{
    [TestFixture]
    public class CSharpKeywordTypeHelperNameRuleTests
    {
        private ITypeHelper _TypeHelper;
        private ITypeNameRule _Rule;

        [SetUp]
        public void SetUp()
        {
            _TypeHelper = Substitute.For<ITypeHelper>();
            _TypeHelper.NameOf(Arg.Any<Type>(), Arg.Any<NameOfTypeOptions>()).Returns(ci => ((Type)ci[0]).FullName);
            
            _Rule = new CSharpKeywordTypeNameRule();
        }

        [Test]
        public void TryGetNameOfType_NullType_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _Rule.TryGetNameOfType(null, _TypeHelper, NameOfTypeOptions.Default));
        }

        [Test]
        public void TryGetNameOfType_NullTypeHelper_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _Rule.TryGetNameOfType(typeof(string), null, NameOfTypeOptions.Default));
        }

        [Test]
        [TestCase(typeof(sbyte), "sbyte")]
        [TestCase(typeof(byte), "byte")]
        [TestCase(typeof(short), "short")]
        [TestCase(typeof(ushort), "ushort")]
        [TestCase(typeof(int), "int")]
        [TestCase(typeof(uint), "uint")]
        [TestCase(typeof(long), "long")]
        [TestCase(typeof(ulong), "ulong")]
        [TestCase(typeof(char), "char")]
        [TestCase(typeof(string), "string")]
        [TestCase(typeof(double), "double")]
        [TestCase(typeof(float), "float")]
        [TestCase(typeof(bool), "bool")]
        public void NameOf_CSharpKeywordTypes_ProducesTheRightResults(Type type, string expected)
        {
            var name = _Rule.TryGetNameOfType(type, _TypeHelper, NameOfTypeOptions.UseCSharpKeywords);

            Assert.That(name, Is.EqualTo(expected));
        }
    }
}