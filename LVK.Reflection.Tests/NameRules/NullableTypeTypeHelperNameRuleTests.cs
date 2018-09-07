using System;
using System.Collections.Generic;

using LVK.Reflection.NameRules;

using NSubstitute;
using NSubstitute.ClearExtensions;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException

namespace LVK.Reflection.Tests.NameRules
{
    [TestFixture]
    public class NullableTypeTypeHelperNameRuleTests
    {
        private ITypeHelper _TypeHelper;
        private ITypeNameRule _Rule;

        [SetUp]
        public void SetUp()
        {
            _TypeHelper = Substitute.For<ITypeHelper>();
            _TypeHelper.NameOf(Arg.Any<Type>(), Arg.Any<NameOfTypeOptions>()).Returns(ci => ((Type)ci[0]).FullName);
            
            _Rule = new NullableTypeNameRule();
        }

        [Test]
        public void TryGetNameOfType_NullType_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _Rule.TryGetNameOfType(null, _TypeHelper, NameOfTypeOptions.UseShorthandSyntax));
        }

        [Test]
        public void TryGetNameOfType_NotGenericType_ReturnsNull()
        {
            var output = _Rule.TryGetNameOfType(typeof(string), _TypeHelper, NameOfTypeOptions.UseShorthandSyntax);

            Assert.That(output, Is.Null);
            _TypeHelper.DidNotReceive().TryGetNameOf(Arg.Any<Type>(), Arg.Any<NameOfTypeOptions>());
        }

        [Test]
        public void TryGetNameOfType_GenericTypeOtherThanNullable_ReturnsNull()
        {
            var output = _Rule.TryGetNameOfType(typeof(List<int>), _TypeHelper, NameOfTypeOptions.UseShorthandSyntax);

            Assert.That(output, Is.Null);
            _TypeHelper.DidNotReceive().TryGetNameOf(Arg.Any<Type>(), Arg.Any<NameOfTypeOptions>());
        }

        [Test]
        public void TryGetNameOfType_NullableTypesButNotShortHandSyntax_ReturnsNull()
        {
            var output = _Rule.TryGetNameOfType(typeof(int?), _TypeHelper, NameOfTypeOptions.None);

            Assert.That(output, Is.Null);
            _TypeHelper.DidNotReceive().TryGetNameOf(Arg.Any<Type>(), Arg.Any<NameOfTypeOptions>());
        }

        [Test]
        public void TryGetNameOfType_NullableTypeAndShorthandSyntax_CallsTypeHelperForUnderlyingName()
        {
            _Rule.TryGetNameOfType(typeof(int?), _TypeHelper, NameOfTypeOptions.UseShorthandSyntax);

            _TypeHelper.Received().TryGetNameOf(typeof(int), Arg.Any<NameOfTypeOptions>());
        }

        [Test]
        public void TryGetNameOfType_NullableTypeAndShorthandSyntax_ReturnsUnderlyingTypePlusQuestionMark()
        {
            _TypeHelper.ClearSubstitute();
            _TypeHelper.TryGetNameOf(Arg.Any<Type>(), Arg.Any<NameOfTypeOptions>()).Returns("DUMMYTYPE");
            
            var output = _Rule.TryGetNameOfType(typeof(int?), _TypeHelper, NameOfTypeOptions.UseShorthandSyntax);

            Assert.That(output, Is.EqualTo("DUMMYTYPE?"));
        }
    }
}