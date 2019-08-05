using JetBrains.Annotations;

namespace LVK.Configuration.Preferences
{
    [PublicAPI]
    public interface IPreference<T>
    {
        [NotNull]
        string Name { get; }

        [CanBeNull]
        T Value { get; set; }

        void Reset();
        void Reload();
        void Save();
    }
}