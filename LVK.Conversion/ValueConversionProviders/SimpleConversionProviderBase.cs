using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Conversion.ValueConversionProviders
{
    internal class SimpleConversionProviderBase : IValueConversionProvider
    {
        [NotNull]
        private readonly Dictionary<(Type sourceType, Type targetType), Func<object, IFormatProvider, object>> _Converters = new Dictionary<(Type sourceType, Type targetType), Func<object, IFormatProvider, object>>();

        protected void AddConverter<TSource, TTarget>([NotNull] Func<TSource, IFormatProvider, TTarget> converter)
        {
            _Converters.Add((typeof(TSource), typeof(TTarget)), (obj, formatProvider) => converter((TSource)obj, formatProvider));
        }

        public Func<object, IFormatProvider, object> TryGetConverter(Type sourceType, Type targetType, IValueConverter valueConverter)
        {
            _Converters.TryGetValue((sourceType, targetType), out Func<object, IFormatProvider, object> converter);
            return converter;
        }
    }
}