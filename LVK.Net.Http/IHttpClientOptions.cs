using JetBrains.Annotations;

namespace LVK.Net.Http
{
    [PublicAPI]
    public interface IHttpClientOptions
    {
        [NotNull]
        IHttpClientOptions Clone();

        [NotNull]
        string BaseUrl { get; set; }
    }
}