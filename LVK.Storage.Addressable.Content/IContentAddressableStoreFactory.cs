using JetBrains.Annotations;

namespace LVK.Storage.Addressable.Content
{
    internal interface IContentAddressableStoreFactory
    {
        [CanBeNull]
        IContentAddressableStore Create([NotNull] string name);
    }
}