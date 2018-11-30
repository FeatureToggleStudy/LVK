using System;

using JetBrains.Annotations;

namespace LVK.Conversion
{
    [PublicAPI]
    public interface IValueConverter
    {
        [CanBeNull]
        Func<object, IFormatProvider, object> TryGetConverter([NotNull] Type sourceType, [NotNull] Type targetType);
    }
}