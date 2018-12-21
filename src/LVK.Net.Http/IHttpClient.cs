using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    [PublicAPI]
    public interface IHttpClient
    {
        [NotNull, ItemNotNull]
        Task<HttpResponseMessage> SendRequestAsync([NotNull] HttpRequestMessage request, CancellationToken? cancellationToken = null);
    }
}