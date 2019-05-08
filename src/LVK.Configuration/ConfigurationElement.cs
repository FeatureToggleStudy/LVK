using System;
using System.Diagnostics;

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Configuration
{
    internal class ConfigurationElement<T> : IConfigurationElement<T>
    {
        [NotNull]
        private readonly Func<JToken> _GetElement;

        [NotNull]
        private readonly JsonSerializer _Serializer;

        [CanBeNull]
        private JToken _LastElement;

        [CanBeNull]
        private T _LastValue;

        public ConfigurationElement([NotNull] Func<JToken> getElement, [NotNull] JsonSerializer serializer)
        {
            _GetElement = getElement ?? throw new ArgumentNullException(nameof(getElement));
            _Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public T Value()
        {
            JToken element = _GetElement();
            if (element == null)
                return default;

            if (ReferenceEquals(element, _LastElement))
                return _LastValue;
            
            T result;
            if (element is JObject obj)
            {
                try
                {
                    result = obj.ToObject<T>(_Serializer);
                }
                catch (JsonReaderException)
                {
                    return default;
                }
                catch (ArgumentException)
                {
                    return default;
                }
            }
            else
                result = element.ToObject<T>(_Serializer);

            _LastElement = element;
            _LastValue = result;

            assume(result != null);
            return result;
        }
    }
}