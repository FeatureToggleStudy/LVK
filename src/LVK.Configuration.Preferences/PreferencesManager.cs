using System;

using JetBrains.Annotations;

using LVK.Data.Caching;

namespace LVK.Configuration.Preferences
{
    internal class PreferencesManager : IPreferencesManager
    {
        [NotNull]
        private readonly IConfiguration _Configuration;

        [NotNull]
        private readonly IPreferencesStore _PreferencesStore;

        [NotNull]
        private readonly IWeakCache<string, object> _PreferencesCache;

        public PreferencesManager([NotNull] IConfiguration configuration, [NotNull] IPreferencesStore preferencesStore, [NotNull] IWeakCache<string, object> preferencesCache)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _PreferencesStore = preferencesStore ?? throw new ArgumentNullException(nameof(preferencesStore));
            _PreferencesCache = preferencesCache ?? throw new ArgumentNullException(nameof(preferencesCache));
        }

        public IPreference<T> GetPreference<T>(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return (IPreference<T>)_PreferencesCache.GetOrAddValue(
                name, _ =>
                {
                    var defaultPreference = _Configuration[$"Preferences/{name}"].Element<T>();
                    return new Preference<T>(name, defaultPreference, _PreferencesStore);
                });
        }
    }
}