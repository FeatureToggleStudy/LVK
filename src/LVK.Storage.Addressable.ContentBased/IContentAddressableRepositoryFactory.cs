using JetBrains.Annotations;

namespace LVK.Storage.Addressable.ContentBased
{
    [PublicAPI]
    public interface IContentAddressableRepositoryFactory
    {
        [CanBeNull]
        IContentAddressableRepository TryCreate([NotNull] string name);
    }
}