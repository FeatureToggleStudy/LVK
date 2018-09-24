using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

using LVK.Core;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException

namespace LVK.Tests.Framework
{
    public abstract class PublicApiTestsBase<T>
    {
        public static IEnumerable<Type> PublicTypes()
            =>
                from type in typeof(T).Assembly.GetTypes()
                where type.IsPublic
                select type;

        public static IEnumerable<ConstructorInfo> PublicConstructors()
            =>
                from type in PublicTypes()
                from constructor in type.GetConstructors()
                where constructor.IsPublic
                select constructor;

        public static IEnumerable<MethodInfo> PublicMethods()
            =>
                from type in PublicTypes()
                from method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                where method.GetCustomAttribute<CompilerGeneratedAttribute>() == null
                where (method.Attributes & MethodAttributes.SpecialName) == 0
                where method.DeclaringType == type
                select method;

        public static IEnumerable<PropertyInfo> PublicProperties()
            =>
                from type in PublicTypes()
                from property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                where property.DeclaringType == type
                select property;

        private static bool IsNullableType(Type type) => type.IsClass || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));

        public static IEnumerable<MethodInfo> PublicMethodsWithNullableReturnTypes()
            =>
                from method in PublicMethods()
                where IsNullableType(method.ReturnType)
                select method;

        public static IEnumerable<PropertyInfo> PublicPropertiesWithNullableTypes()
            =>
                from property in PublicProperties()
                where IsNullableType(property.PropertyType)
                select property;

        public static IEnumerable<ParameterInfo> ParametersOfPublicMethods()
        {
            var constructorParameters =
                from constructor in PublicConstructors()
                from parameter in constructor.GetParameters()
                select parameter;

            var methodParameters =
                from method in PublicMethods()
                where !IsImplementingInterfaceMethod(method)
                from parameter in method.GetParameters()
                select parameter;

            return constructorParameters.Concat(methodParameters);
        }

        private static bool IsImplementingInterfaceMethod(MethodInfo method)
            => (
                from intf in method.DeclaringType.GetInterfaces()
                from implementingMethod in method.DeclaringType.GetInterfaceMap(intf).TargetMethods
                where implementingMethod == method
                select implementingMethod).Any();

        public static IEnumerable<ParameterInfo> ParametersOfPublicMethodsWithNullableTypes()
            =>
                from parameter in ParametersOfPublicMethods()
                where IsNullableType(parameter.ParameterType)
                select parameter;

        [Test]
        [TestCaseSource(nameof(PublicTypes))]
        public void PublicType_IsTaggedWithPublicApi(Type publicType)
        {
            var isPublicApi = publicType.IsDefined(typeof(PublicAPIAttribute), false);
            Assert.That(isPublicApi, Is.True, $"Type {publicType} does not have [PublicAPI]");
        }

        [Test]
        [TestCaseSource(nameof(PublicMethodsWithNullableReturnTypes))]
        public void NullableReturnValues_IsTaggedWithNotNullOrCanBeNull(MethodInfo method)
        {
            var hasNotNull = method.GetCustomAttribute<NotNullAttribute>() != null;
            var hasCanBeNull = method.GetCustomAttribute<CanBeNullAttribute>() != null;

            Assert.That(hasCanBeNull || hasNotNull, Is.True, $"Method {method.DeclaringType}.{method.Name} does not have [CanBeNull] or [NotNull]");
        }

        [Test]
        [TestCaseSource(nameof(PublicPropertiesWithNullableTypes))]
        public void NullableProperties_IsTaggedWithNotNullOrCanBeNull(PropertyInfo property)
        {
            var hasNotNull = property.GetCustomAttribute<NotNullAttribute>() != null;
            var hasCanBeNull = property.GetCustomAttribute<CanBeNullAttribute>() != null;

            Assert.That(hasCanBeNull || hasNotNull, Is.True, $"Property {property.DeclaringType}.{property.Name} does not have [CanBeNull] or [NotNull]");
        }

        [Test]
        [TestCaseSource(nameof(ParametersOfPublicMethodsWithNullableTypes))]
        public void NullableParameters_IsTaggedWithNotNullOrCanBeNull(ParameterInfo parameter)
        {
            var hasNotNull = parameter.GetCustomAttribute<NotNullAttribute>() != null;
            var hasCanBeNull = parameter.GetCustomAttribute<CanBeNullAttribute>() != null;
            var hasContractAnnotation = parameter.Member.GetCustomAttribute<ContractAnnotationAttribute>() != null;

            Assert.That(hasCanBeNull || hasNotNull || hasContractAnnotation, Is.True, $"Parameter {parameter.Name} of {parameter.Member.DeclaringType}.{parameter.Member.Name} does not have [CanBeNull] or [NotNull]");
        }
    }
}