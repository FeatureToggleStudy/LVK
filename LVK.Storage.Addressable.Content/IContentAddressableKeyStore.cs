using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Storage.Addressable.Content
{
    internal interface IContentAddressableKeyStore
    {
        ContentAddressableKey GetKey([NotNull] string name);

        void PutKey([NotNull] string name, ContentAddressableKey key);

        [NotNull]
        IEnumerable<ContentAddressableKey> GetStoredKeys();
    }
}