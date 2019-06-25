using System;

using JetBrains.Annotations;

namespace LVK.Configuration.Preferences
{
    internal class Preference<T> : IPreference<T>
    {
        [NotNull]
        private readonly IConfigurationElement<T> _DefaultPreference;

        [NotNull]
        private readonly IPreferencesStore _PreferencesStore;

        [NotNull]
        private readonly object _Lock = new object();
        
        public Preference([NotNull] string name, [NotNull] IConfigurationElement<T> defaultPreference, [NotNull] IPreferencesStore preferencesStore)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _DefaultPreference = defaultPreference ?? throw new ArgumentNullException(nameof(defaultPreference));
            _PreferencesStore = preferencesStore ?? throw new ArgumentNullException(nameof(preferencesStore));

            Reload();
        }

        public string Name { get; }

        public T Value { get; set; }

        public void Reset()
        {
            lock (_Lock)
            {
                _PreferencesStore.Delete(Name);
                Value = _DefaultPreference.ValueOrDefault();
            }
        }

        public void Reload()
        {
            lock (_Lock)
            {
                var (success, value) = _PreferencesStore.TryGetValue<T>(Name);
                Value = success ? value : _DefaultPreference.ValueOrDefault();
            }
        }
        

        public void Save()
        {
            lock (_Lock)
            {
                _PreferencesStore.SetValue(Name, Value);
            }
        }
    }
}