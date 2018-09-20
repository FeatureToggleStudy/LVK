using System;

using JetBrains.Annotations;

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
            try
            {
                return _Element.Value();
            }
            catch (ArgumentException)
            {
                return _GetDefaultValue();
            }
            catch (InvalidCastException)
            {
                return _GetDefaultValue();
            }
        }
    }
}