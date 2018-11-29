using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Storage.Addressable.PathBased;

using Microsoft.AspNetCore.Mvc;

namespace WindowsServiceSandbox.Controllers
{
    public class TestController : Controller
    {
        [CanBeNull]
        private readonly IPathAddressableRepository _Repository;

        public TestController([NotNull] IPathAddressableRepositoryFactory repositoryFactory)
        {
            if (repositoryFactory == null)
                throw new ArgumentNullException(nameof(repositoryFactory));

            _Repository = repositoryFactory.TryCreate("main");
        }

        [HttpGet]
        [Route("/content/{id}")]
        public async Task<string> GetContent(string id)
        {
            if (_Repository is null)
                throw new InvalidOperationException("No repository configured");

            return await _Repository.TryGetObjectAsync<string>(id);
        }

        [HttpPost]
        [Route("/content/{id}")]
        public Task PostContent(string id, [FromBody] string contents)
        {
            if (_Repository is null)
                throw new InvalidOperationException("No repository configured");

            return _Repository.StoreObjectAsync(id, contents);
        }
    }
}