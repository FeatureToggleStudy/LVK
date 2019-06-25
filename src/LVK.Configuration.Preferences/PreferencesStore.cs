using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration.Preferences
{
    internal class PreferencesStore : IPreferencesStore
    {
        [NotNull]
        private readonly IPreferencesFile _PreferencesFile;

        [CanBeNull]
        private JObject _Preferences;

        [NotNull]
        private readonly object _Lock = new object();

        public PreferencesStore([NotNull] IPreferencesFile preferencesFile)
        {
            _PreferencesFile = preferencesFile ?? throw new ArgumentNullException(nameof(preferencesFile));
        }

        public void Delete(string key)
        {
            lock (_Lock)
            {
                var preferences = Load();
                
                preferences.Remove(key);
                
                Save();
            }
        }

        public void SetValue<T>(string key, T value)
        {
            lock (_Lock)
            {
                var preferences = Load();

                if (value == null)
                    preferences[key] = null;
                else
                    preferences[key] = JToken.FromObject(value);
                
                Save();
            }
        }

        public (bool success, T value) TryGetValue<T>(string key)
        {
            lock (_Lock)
            {
                var preferences = Load();

                JToken value = preferences[key];
                if (value == null)
                    return (false, default);

                return (true, value.Value<T>());
            }
        }

        [NotNull]
        private JObject Load()
        {
            if (_Preferences == null)
                _Preferences = _PreferencesFile.Load();
            
            return _Preferences;
        }

        private void Save()
        {
            if (_Preferences != null)
                _PreferencesFile.Save(_Preferences);
        }
    }
}