using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration
{
    internal interface IConfigurationProvider
    {
        [NotNull]
        JObject GetConfiguration();
    }
}