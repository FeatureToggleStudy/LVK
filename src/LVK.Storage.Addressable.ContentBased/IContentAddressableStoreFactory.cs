using JetBrains.Annotations;

namespace LVK.Storage.Addressable.ContentBased
{
    internal interface IContentAddressableStoreFactory
    {
        [CanBeNull]
        IContentAddressableStore Create([NotNull] string name);
    }
}