using System;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Configuration
{
    internal class ConfigurationElementWithDefaultValue<T> : IConfigurationElement<T>
    {
        [NotNull]
        private readonly IConfigurationElement<T> _Element;

        [NotNull]
        private readonly Func<T> _GetDefaultValue;

        public ConfigurationElementWithDefaultValue([NotNull] IConfigurationElement<T> element, [NotNull] Func<T> getDefaultValue)
        {
            _Element = element ?? throw new ArgumentNullException(nameof(element));
            _GetDefaultValue = getDefaultValue ?? throw new ArgumentNullException(nameof(getDefaultValue));
        }

        public T Value()
        {
            T result;
            try
            {
                result = _Element.Value();
            }
            catch (ArgumentException)
            {
                result = _GetDefaultValue();
            }
            catch (InvalidCastException)
            {
                result = _GetDefaultValue();
            }

            assume(result != null);
            return result;
        }
    }
}