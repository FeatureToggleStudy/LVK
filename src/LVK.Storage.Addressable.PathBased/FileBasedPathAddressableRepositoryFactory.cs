using System;

using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Storage.Addressable.PathBased
{
    internal class FileBasedPathAddressableRepositoryFactory : IPathAddressableRepositoryFactory
    {
        [NotNull]
        private readonly IConfiguration _Configuration;

        public FileBasedPathAddressableRepositoryFactory([NotNull] IConfiguration configuration)
        {
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IPathAddressableRepository TryCreate(string name)
        {
            var configuration = _Configuration[$"Storage/PathAddressable/{name}"]
               .Element<PathAddressableRepositoryConfiguration>()
               .ValueOrDefault(() => new PathAddressableRepositoryConfiguration());

            if (string.IsNullOrWhiteSpace(configuration.BasePath))
                return null;

            return new FileBasedPathAddressableRepository(configuration.BasePath);
        }
    }
}