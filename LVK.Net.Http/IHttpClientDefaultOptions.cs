using System.Net;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    [PublicAPI]
    public interface IHttpClientDefaultOptions
    {
        [NotNull]
        string BaseUrl { get; set; }

        [CanBeNull]
        ICredentials Credentials { get; set; }
    }
}