using JetBrains.Annotations;

namespace LVK.Configuration.Preferences
{
    [PublicAPI]
    public interface IPreferencesManager
    {
        [NotNull]
        IPreference<T> GetPreference<T>([NotNull] string name);
    }
}