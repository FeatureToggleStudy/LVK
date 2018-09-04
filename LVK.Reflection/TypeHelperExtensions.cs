using System;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Reflection
{
    [PublicAPI]
    public static class TypeHelperExtensions
    {
        public static string NameOf(
            this ITypeHelper typeHelper, Type type, NameOfTypeOptions options = NameOfTypeOptions.Default)
            => typeHelper.TryGetNameOf(type, options) ?? type.FullName.NotNull();
    }
}