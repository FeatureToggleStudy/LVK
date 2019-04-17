using System;

using JetBrains.Annotations;

using LVK.Reflection;

namespace LVK.Conversion
{
    [PublicAPI]
    public static class ValueConverterExtensions
    {
        [CanBeNull]
        public static Func<TSource, IFormatProvider, TTarget> TryGetConverter<TSource, TTarget>(
            [NotNull] this IValueConverter valueConverter)
        {
            if (valueConverter is null)
                throw new ArgumentNullException(nameof(valueConverter));

            Func<object, IFormatProvider, object> converter = valueConverter.TryGetConverter(
                typeof(TSource), typeof(TTarget));

            if (converter == null)
                return null;

            return (source, formatProvider) => (TTarget)converter(source, formatProvider);
        }

        public static (bool success, TTarget targetValue) TryConvert<TSource, TTarget>(
            [NotNull] this IValueConverter valueConverter, [CanBeNull] TSource sourceValue,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            if (!TryConvert(valueConverter, sourceValue, out TTarget targetValue, formatProvider))
                return (false, default);

            return (true, targetValue);
        }

        public static bool TryConvert<TSource, TTarget>(
            [NotNull] this IValueConverter valueConverter, [CanBeNull] TSource sourceValue, [CanBeNull] out TTarget targetValue,
            [CanBeNull] IFormatProvider formatProvider = null)
        {
            if (valueConverter is null)
                throw new ArgumentNullException(nameof(valueConverter));

            var converter = valueConverter.TryGetConverter<TSource, TTarget>();
            if (converter == null)
            {
                targetValue = default;
                return false;
            }

            targetValue = converter(sourceValue, formatProvider);
            return true;
        }

        [CanBeNull]
        public static TTarget Convert<TSource, TTarget>([NotNull] this IValueConverter valueConverter, [CanBeNull] TSource sourceValue, [CanBeNull] IFormatProvider formatProvider = null)
        {
            if (!TryConvert(valueConverter, sourceValue, out TTarget targetValue, formatProvider))
                throw new FormatException($"Unable to get converter from '{TypeHelper.Instance.NameOf<TSource>()}' to '{TypeHelper.Instance.NameOf<TTarget>()}'");

            return targetValue;
        }
    }
}