using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using LVK.Configuration.Layers.JsonFile;
using LVK.Configuration.Layers.Static;
using LVK.Core;
using LVK.Json;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration.Layers.CommandLine
{
    internal class CommandLineConfigurationLayersProvider : IConfigurationLayersProvider
    {
        [NotNull]
        private readonly IClock _Clock;

        [NotNull, ItemNotNull]
        private readonly string[] _Arguments;

        public CommandLineConfigurationLayersProvider([NotNull] IClock clock, [NotNull, ItemNotNull] string[] arguments)
        {
            _Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        public IEnumerable<IConfigurationLayer> Provide()
        {
            yield return new StaticConfigurationLayer(
                new JObject { ["CommandLineArguments"] = new JArray(_Arguments.OfType<object>().ToArray()) });

            JObject configuration = null;

            var reExplicitValue = new Regex("--(?<path>[a-z_][a-z0-9_/]*)(=(?<value>.*))?", RegexOptions.IgnoreCase);
            foreach (var argument in _Arguments)
            {
                if (argument.StartsWith("@"))
                {
                    if (configuration != null)
                        yield return new StaticConfigurationLayer(configuration);

                    configuration = null;
                    yield return new RequiredJsonFileConfigurationLayer(_Clock, argument.Substring(1), Encoding.Default);
                }
                else
                {
                    Match ma = reExplicitValue.Match(argument);
                    if (ma.Success)
                    {
                        configuration = configuration ?? new JObject();
                        ApplyCommandLineValueOverride(configuration, ma);
                    }
                }
            }
            if (configuration != null)
                yield return new StaticConfigurationLayer(configuration);
        }
        
        private void ApplyCommandLineValueOverride(JObject configuration, [NotNull] Match match)
        {
            var path = match.Groups["path"]?.Value ?? string.Empty;
            if (string.IsNullOrWhiteSpace(path))
                return;
        
            var group = match.Groups["value"];
            JetBrainsHelpers.assume(group != null);
            
            var stringValue = group.Success ? group.Value : "true";
            JToken value = JsonBuilder.ValueFromString(stringValue);

            var valueConfiguration = JsonBuilder.Construct(path.Split('/'), value);
            JsonBuilder.Apply(valueConfiguration, configuration);
        }
    }
}