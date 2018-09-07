using System;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Reflection
{
    [PublicAPI]
    public static class TypeHelperExtensions
    {
        [NotNull]
        public static string NameOf(
            [NotNull] this ITypeHelper typeHelper, [NotNull] Type type,
            NameOfTypeOptions options = NameOfTypeOptions.Default)
            => typeHelper.TryGetNameOf(type, options) ?? type.FullName.NotNull();

        [NotNull]
        public static string NameOf<T>(
            [NotNull] this ITypeHelper typeHelper, NameOfTypeOptions options = NameOfTypeOptions.Default)
            => NameOf(typeHelper, typeof(T), options);

    }
}