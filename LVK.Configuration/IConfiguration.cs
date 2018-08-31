using JetBrains.Annotations;

namespace LVK.Configuration
{
    public interface IConfiguration
    {
        [NotNull]
        IConfiguration this[[NotNull] string path] { get; }

        [NotNull]
        IConfiguration this[[NotNull, ItemNotNull] string[] path] { get; }

        [CanBeNull]
        T Value<T>();
    }
}