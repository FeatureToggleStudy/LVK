using JetBrains.Annotations;

namespace LVK.Persistence
{
    [PublicAPI]
    public interface IPersistentData<T>
        where T: class, new()
    {
        [NotNull]
        T Value { get; set; }

        void Reload();
        void Save();

        void BeginUpdates();
        void EndUpdates();
    }
}