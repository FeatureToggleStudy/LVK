using JetBrains.Annotations;

namespace LVK.Storage.Addressable.PathBased
{
    [PublicAPI]
    public interface IPathAddressableRepositoryFactory
    {
        [CanBeNull]
        IPathAddressableRepository TryCreate([NotNull] string name);
    }
}