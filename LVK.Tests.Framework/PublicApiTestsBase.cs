using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException

namespace LVK.Tests.Framework
{
    public abstract class PublicApiTestsBase<T>
    {
        public static IEnumerable<Type> PublicTypes() => typeof(T).Assembly.GetTypes().Where(t => t.IsPublic);

        public static IEnumerable<MethodInfo> PublicMethods()
            =>
                from type in PublicTypes()
                from method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                where method.GetCustomAttribute<CompilerGeneratedAttribute>() == null
                where (method.Attributes & MethodAttributes.SpecialName) == 0
                where method.DeclaringType == type
                select method;

        public static IEnumerable<MethodInfo> PublicMethodsWithReferenceTypeReturnTypes() => PublicMethods().Where(method => method.ReturnType.IsClass);

        [Test]
        [TestCaseSource(nameof(PublicTypes))]
        public void PublicType_IsTaggedWithPublicApi(Type publicType)
        {
            var isPublicApi = publicType.IsDefined(typeof(PublicAPIAttribute), false);
            Assert.That(isPublicApi, Is.True, $"Type {publicType} does not have [PublicAPI]");
        }

        [Test]
        [TestCaseSource(nameof(PublicMethodsWithReferenceTypeReturnTypes))]
        public void ReferenceTypeReturnValues_IsTaggedWithNotNullOrCanBeNull(MethodInfo method)
        {
            var hasNotNull = method.GetCustomAttribute<NotNullAttribute>() != null;
            var hasCanBeNull = method.GetCustomAttribute<CanBeNullAttribute>() != null;

            Assert.That(hasCanBeNull || hasNotNull, Is.True, $"Method {method.DeclaringType}.{method.Name} does not have [CanBeNull] or [NotNull]");
        }
    }
}