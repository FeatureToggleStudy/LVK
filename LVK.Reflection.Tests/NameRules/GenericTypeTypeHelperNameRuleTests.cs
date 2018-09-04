using System;
using System.Collections.Generic;

using LVK.Reflection.NameRules;

using NSubstitute;
using NSubstitute.ClearExtensions;

using NUnit.Framework;

namespace LVK.Reflection.Tests.NameRules
{
    [TestFixture]
    public class GenericTypeTypeHelperNameRuleTests
    {
        private ITypeHelper _TypeHelper;
        private ITypeNameRule _Rule;

        [SetUp]
        public void SetUp()
        {
            _TypeHelper = Substitute.For<ITypeHelper>();
            _TypeHelper.NameOf(Arg.Any<Type>(), Arg.Any<NameOfTypeOptions>()).Returns(ci => ((Type)ci[0]).FullName);
            
            _Rule = new GenericTypeNameRule();
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
        public void TryGetNameOfType_NonGenericType_ReturnsNull()
        {
            var output = _Rule.TryGetNameOfType(typeof(string), _TypeHelper, NameOfTypeOptions.None);

            Assert.That(output, Is.Null);
        }

        [Test]
        public void TryGetNameOfType_GenericType_CallsTypeHelperToGetUnderlyingNames()
        {
            _TypeHelper.ClearSubstitute();
            _TypeHelper.TryGetNameOf(typeof(int), Arg.Any<NameOfTypeOptions>()).Returns("int");

            _Rule.TryGetNameOfType(typeof(List<int>), _TypeHelper, NameOfTypeOptions.Default);

            _TypeHelper.Received().TryGetNameOf(typeof(int), Arg.Any<NameOfTypeOptions>());
        }

        [Test]
        [TestCase(false, "Dictionary<int,string>")]
        [TestCase(true, "System.Collections.Generic.Dictionary<int,string>")]
        public void TryGetNameOfType_GenericTypeNoNamespace_ReturnsCorrectResults(bool includeNamespaces, string expected)
        {
            _TypeHelper.ClearSubstitute();
            _TypeHelper.TryGetNameOf(typeof(int), Arg.Any<NameOfTypeOptions>()).Returns("int");
            _TypeHelper.TryGetNameOf(typeof(string), Arg.Any<NameOfTypeOptions>()).Returns("string");

            var options = NameOfTypeOptions.None;
            if (includeNamespaces)
                options |= NameOfTypeOptions.IncludeNamespaces;
            
            var output = _Rule.TryGetNameOfType(typeof(Dictionary<int, string>), _TypeHelper, options);

            Assert.That(output, Is.EqualTo(expected));
        }
    }
}