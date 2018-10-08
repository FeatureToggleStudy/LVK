using JetBrains.Annotations;

namespace LVK.Storage.Addressable.Content
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