using System;

using LVK.Core;

namespace LVK.Reflection.NameRules
{
    internal class NullableTypeNameRule : ITypeNameRule
    {
        public int Priority => 2;

        public string TryGetNameOfType(Type type, ITypeHelper typeHelper, NameOfTypeOptions options)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (typeHelper is null)
                throw new ArgumentNullException(nameof(typeHelper));

            if ((options & NameOfTypeOptions.UseShorthandSyntax) == 0)
                return null;

            if (!(type.IsGenericType))
                return null;

            Type openGenericType = type.GetGenericTypeDefinition();
            if (openGenericType != typeof(Nullable<>))
                return null;

            Type underlyingType = type.GetGenericArguments()[0].NotNull();
            return typeHelper.NameOf(underlyingType) + "?";
        }
    }
}