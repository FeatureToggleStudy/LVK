using JetBrains.Annotations;

namespace LVK.ContentAddressableStorage
{
    internal interface IContentAddressableStoreFactory
    {
        [CanBeNull]
        IContentAddressableStore Create([NotNull] string name);
    }
}