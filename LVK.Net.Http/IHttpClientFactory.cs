using JetBrains.Annotations;

namespace LVK.Net.Http
{
    [PublicAPI]
    public interface IHttpClientFactory
    {
        [NotNull]
        IHttpClient Create([NotNull] IHttpClientOptions options);

        [NotNull]
        IHttpClient CreateDefault();
    }
}