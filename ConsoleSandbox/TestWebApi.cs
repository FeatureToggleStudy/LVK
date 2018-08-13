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

        public TestWebApi([NotNull] IHttpClientFactory clientFactory, [NotNull] IHttpClientOptions options)
        {
            options.BaseUrl = "http://localhost:5000/api";
            _Client = clientFactory.Create(options);
        }

        public Task<string[]> GetValuesAsync(CancellationToken cancellationToken)
            => _Client.GetAsync<string[]>("values", cancellationToken);

        public Task<string> GetValueAsync(int id, CancellationToken cancellationToken)
            => _Client.GetStringAsync($"values/{id}", cancellationToken);

        public Task PostAsync(string value, CancellationToken cancellationToken)
            => _Client.PostAsync("values", value, cancellationToken);

        public Task PutAsync(int id, string value, CancellationToken cancellationToken)
            => _Client.PutAsync($"values/{id}", value, cancellationToken);

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
            => _Client.DeleteAsync($"values/{id}", cancellationToken);
    }
}