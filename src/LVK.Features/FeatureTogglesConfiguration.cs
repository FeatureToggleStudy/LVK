using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LVK.Features
{
    internal class FeatureTogglesConfiguration
    {
        [JsonExtensionData]
        public JObject Root { get; set; }
    }
}