using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Net.Http;

namespace ConsoleSandbox
{
    public class TestWebApi : ITestWebApi
    {
        [NotNull]
        private readonly IHttpClient _Client;

        public TestWebApi([NotNull] IHttpClientFactory clientFactory)
        {
            _Client = clientFactory.CreateDefault();
            _Client.Options.BaseUrl = "http://localhost:5000/api";
        }

        public Task<string[]> GetValuesAsync(CancellationToken cancellationToken) => _Client.GetAsync<string[]>("values", cancellationToken);
    }
}