using System;
using System.Net;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    [UsedImplicitly]
    internal class HttpClientOptions : IHttpClientOptions
    {
        private string _BaseUrl = string.Empty;

        public HttpClientOptions()
        {
        }

        public HttpClientOptions([NotNull] IHttpClientDefaultOptions defaultOptions)
        {
            if (defaultOptions is null)
                throw new ArgumentNullException(nameof(defaultOptions));

            BaseUrl = defaultOptions.BaseUrl;
            Credentials = defaultOptions.Credentials;
        }

        public string BaseUrl
        {
            get => _BaseUrl;
            // ReSharper disable once ConstantNullCoalescingCondition
            set => _BaseUrl = EnsureEndsWithSlash(value ?? string.Empty);
        }

        public ICredentials Credentials { get; set; }

        [NotNull]
        private string EnsureEndsWithSlash([NotNull] string url)
        {
            if (!url.EndsWith("/"))
                return url + "/";

            return url;
        }
    }
}