using System;
using System.Text;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration
{
    [PublicAPI]
    public interface IConfigurationBuilder
    {
        [NotNull]
        IConfiguration Build();

        void AddJsonFile([NotNull] string filename, [CanBeNull] Encoding encoding = null, bool isOptional = false);
        void AddJson([NotNull] string json);
        void AddDynamic([NotNull] Func<JObject> getConfiguration);
        void AddEnvironmentVariables([NotNull] string prefix);
        void AddCommandLine([NotNull, ItemNotNull] string[] args);
    }
}