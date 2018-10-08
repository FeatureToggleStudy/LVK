using JetBrains.Annotations;

namespace LVK.Storage.Addressable.ContentBased
{
    [UsedImplicitly]
    internal class ContentAddressableStoreConfiguration
    {
        public string BasePath
        {
            get;
            [UsedImplicitly]
            set;
        }
    }
}