using System;
using System.Net.Http;

using JetBrains.Annotations;

using LVK.Net.Http;

namespace ConsoleSandbox
{
    public class TestWebApi : RepositoryWebApiClient<int, string>, ITestWebApi
    {
        public TestWebApi([NotNull] HttpClient client)
            : base(client, new Uri("http://localhost:5000/api/values/"))
        {
        }
    }
}