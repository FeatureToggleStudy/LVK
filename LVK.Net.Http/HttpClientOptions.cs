using System;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    [UsedImplicitly]
    internal class HttpClientOptions : IHttpClientOptions
    {
        private string _BaseUrl = string.Empty;

        public IHttpClientOptions Clone()
        {
            var clone = new HttpClientOptions { BaseUrl = BaseUrl };
            return clone;
        }

        public string BaseUrl
        {
            get => _BaseUrl;
            set => _BaseUrl = EnsureEndsWithSlash(value ?? string.Empty);
        }

        [NotNull]
        private string EnsureEndsWithSlash([NotNull] string url)
        {
            if (!url.EndsWith("/"))
                return url + "/";

            return url;
        }
    }
}