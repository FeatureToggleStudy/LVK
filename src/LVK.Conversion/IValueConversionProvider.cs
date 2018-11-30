using System;

using JetBrains.Annotations;

namespace LVK.Conversion
{
    [PublicAPI]
    public interface IValueConversionProvider
    {
        [CanBeNull]
        Func<object, IFormatProvider, object> TryGetConverter(
            [NotNull] Type sourceType, [NotNull] Type targetType, [NotNull] IValueConverter valueConverter);
    }
}