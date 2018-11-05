using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;

namespace LVK.Configuration
{
    internal class VariableConfigurationDecoder : IConfigurationDecoder
    {
        [NotNull]
        private readonly Regex _Pattern;

        [NotNull, ItemNotNull]
        private ILookup<string, IConfigurationVariables> _ConfigurationVariablesByKey;

        public VariableConfigurationDecoder([NotNull, ItemNotNull] IEnumerable<IConfigurationVariables> configurationVariables)
        {
            _ConfigurationVariablesByKey = configurationVariables.ToLookup(cv => cv.Prefix);
            var prefixes = string.Join("|", _ConfigurationVariablesByKey.Select(cv => cv.NotNull().Key));
            _Pattern = new Regex($@"\$\{{(?<key>(?<prefix>{prefixes})\.[^}}]+)\}}");
        }

        public Type SupportedType => typeof(string);

        public object Decode(object value)
        {
            if (!(value is string input))
                return value;

            return _Pattern.Replace(input, ReplaceVariable);
        }

        private string ReplaceVariable([NotNull] Match match)
        {
            var prefix = match.Groups["prefix"].NotNull().Value;
            var configurationVariablesForKey = _ConfigurationVariablesByKey[prefix];
            
            var key = match.Groups["key"].NotNull().Value;
            foreach (var configurationVariables in configurationVariablesForKey)
            {
                var (success, value) = configurationVariables.TryGetValue(key);
                if (success)
                    return value;
            }

            return match.Value;
        }
    }
}