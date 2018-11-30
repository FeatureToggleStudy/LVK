using JetBrains.Annotations;

namespace LVK.Configuration
{
    [PublicAPI]
    public interface IConfiguration
    {
        [NotNull]
        IConfiguration this[[NotNull] string path] { get; }

        [NotNull]
        IConfiguration this[[NotNull, ItemNotNull] string[] path] { get; }

        [NotNull]
        IConfigurationElement<T> Element<T>();
    }
}