using System.Threading.Tasks;

namespace LVK.ContentAddressableStorage
{
    internal interface IContentAddressableStore : IContentAddressableObjectStore, IContentAddressableKeyStore
    {
    }
}