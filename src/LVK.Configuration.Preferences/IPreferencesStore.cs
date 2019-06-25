using JetBrains.Annotations;

namespace LVK.Configuration.Preferences
{
    internal interface IPreferencesStore
    {
        void Delete([NotNull] string key);
        void SetValue<T>(string key, [CanBeNull] T value);
        (bool success, T value) TryGetValue<T>([NotNull] string key);
    }
}