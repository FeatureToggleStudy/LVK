using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration.Layers.Static
{
    internal class StaticConfigurationLayer : IConfigurationLayer
    {
        public StaticConfigurationLayer([NotNull] JObject configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [NotNull]
        public JObject Configuration { get; }
    }
}