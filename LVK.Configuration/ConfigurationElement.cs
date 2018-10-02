using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

using static LVK.Core.JetBrainsHelpers;

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
            JToken element = _GetElement();
            if (element == null)
                return default;
                
            T result;
            if (element is JObject obj)
            {
                try
                {
                    result = obj.ToObject<T>();
                }
                catch (ArgumentException)
                {
                    return default(T);
                }
            }
            else
                result = element.ToObject<T>();

            assume(result != null);
            return result;
        }
    }
}