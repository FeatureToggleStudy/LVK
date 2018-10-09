using JetBrains.Annotations;

namespace LVK.Storage.Addressable.PathBased
{
    internal class PathAddressableRepositoryConfiguration
    {
        [CanBeNull]
        public string BasePath { get; set; }
    }
}