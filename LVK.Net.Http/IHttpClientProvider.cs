using System.Net.Http;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    [PublicAPI]
    public interface IHttpClientProvider
    {
        [NotNull]
        HttpClient Provide([NotNull] string name);
    }
}