using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.ContentAddressableStorage
{
    internal interface IContentAddressableKeyStore
    {
        ContentAddressableKey GetKey([NotNull] string name);

        void PutKey([NotNull] string name, ContentAddressableKey key);

        [NotNull]
        IEnumerable<ContentAddressableKey> GetStoredKeys();
    }
}