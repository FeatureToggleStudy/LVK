using JetBrains.Annotations;

namespace LVK.ContentAddressableStorage
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