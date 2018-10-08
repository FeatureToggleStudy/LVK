using JetBrains.Annotations;

namespace LVK.Storage.Addressable.Content
{
    [PublicAPI]
    public interface IContentAddressableRepositoryFactory
    {
        [CanBeNull]
        IContentAddressableRepository Create([NotNull] string name);
    }
}