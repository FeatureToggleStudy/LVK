using System.Text;

using JetBrains.Annotations;

namespace LVK.Configuration
{
    [PublicAPI]
    public interface IConfigurationBuilder
    {
        void SetBasePath([NotNull] string basePath);

        void AddJsonFile([NotNull] string filename, [CanBeNull] Encoding encoding = null,
                         bool isOptional = false);

        void AddJson([NotNull] string json);
        void AddEnvironmentVariables([NotNull] string prefix);
        void AddCommandLine([NotNull, ItemNotNull] string[] args);
    }
}