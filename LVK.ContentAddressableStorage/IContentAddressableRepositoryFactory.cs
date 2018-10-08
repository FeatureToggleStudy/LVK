using JetBrains.Annotations;

namespace LVK.ContentAddressableStorage
{
    [PublicAPI]
    public interface IContentAddressableRepositoryFactory
    {
        [CanBeNull]
        IContentAddressableRepository Create([NotNull] string name);
    }
}