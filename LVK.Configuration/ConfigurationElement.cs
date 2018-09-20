using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration
{
    internal class ConfigurationElement<T> : IConfigurationElement<T>
    {
        [NotNull]
        private readonly Func<JToken> _GetElement;

        public ConfigurationElement([NotNull] Func<JToken> getElement)
        {
            _GetElement = getElement ?? throw new ArgumentNullException(nameof(getElement));
        }

        public T Value()
        {
            var element = _GetElement();
            if (!(element is JObject obj))
                return element.ToObject<T>();

            return obj.ToObject<T>();
        }
    }
}