using JetBrains.Annotations;

namespace LVK.Configuration
{
    [PublicAPI]
    public static class ConfigurationExtensions
    {
        [NotNull]
        public static IConfigurationElement<T> Element<T>([NotNull] this IConfiguration configuration, [NotNull] string path)
            => configuration[path].Element<T>();

        [NotNull]
        public static IConfigurationElement<T> Element<T>([NotNull] this IConfiguration configuration, [NotNull, ItemNotNull] string[] path)
            => configuration[path].Element<T>();
    }
}