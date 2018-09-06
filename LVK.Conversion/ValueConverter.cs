using System;
using System.Collections.Generic;
using System.Linq;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Conversion
{
    [PublicAPI]
    public class ValueConverter : IValueConverter
    {
        [NotNull]
        private readonly Dictionary<(Type sourceType, Type targetType), Func<object, IFormatProvider, object>>
            _ValueConverters =
                new Dictionary<(Type sourceType, Type targetType), Func<object, IFormatProvider, object>>();

        [NotNull]
        private readonly object _Lock = new object();

        [NotNull, ItemNotNull]
        private readonly List<IValueConversionProvider> _ValueConversionProviders;

        [NotNull]
        private readonly Func<object, IFormatProvider, object> _NoConversionConverter;

        [NotNull]
        private static readonly object _InstanceLock = new object();

        [CanBeNull]
        private static IValueConverter _Instance;

        public ValueConverter([NotNull] IEnumerable<IValueConversionProvider> valueConversionProviders)
        {
            if (valueConversionProviders is null)
                throw new ArgumentNullException(nameof(valueConversionProviders));

            _ValueConversionProviders = valueConversionProviders.ToList();
            _NoConversionConverter = (o, fp) => o;
            Instance = this;
        }

        [NotNull]
        public static IValueConverter Instance
        {
            get
            {
                if (_Instance is null)
                {
                    lock (_InstanceLock)
                    {
                        if (_Instance is null)
                        {
                            // ReSharper disable once HeuristicUnreachableCode
                            new Container().Bootstrap<ServicesBootstrapper>().Resolve<IValueConverter>();
                        }
                    }

                    assume(_Instance != null);
                }

                return _Instance;
            }
            internal set => _Instance = value;
        }

        public Func<object, IFormatProvider, object> TryGetConverter(Type sourceType, Type targetType)
        {
            if (sourceType == targetType)
                return _NoConversionConverter;
            
            Func<object, IFormatProvider, object> converter;

            var key = (sourceType, targetType);
            lock (_Lock)
            {
                if (!_ValueConverters.TryGetValue(key, out converter))
                {
                    converter = TryFindConverter(sourceType, targetType);
                    _ValueConverters[key] = converter;
                }
            }

            return converter;
        }

        private Func<object, IFormatProvider, object> TryFindConverter(
            [NotNull] Type sourceType, [NotNull] Type targetType)
        {
            var converters =
                from valueConversionProvider in _ValueConversionProviders
                let converter = valueConversionProvider.TryGetConverter(sourceType, targetType, this)
                where converter != null
                select converter;

            return converters.FirstOrDefault();
        }
    }
}