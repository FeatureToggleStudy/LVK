using System;

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

            assume(result != null);
            return result;
        }
    }
}