using System;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    internal class EnvironmentConfigurationVariables : IConfigurationVariables
    {
        [NotNull]
        private static readonly Regex _Pattern = new Regex(@"^env\.(?<key>.*)$");
        
        public string Prefix => "env";

        public (bool success, string value) TryGetValue(string name)
        {
            var match = _Pattern.Match(name);
            if (!match.Success)
                return (false, default);
            
            var value = Environment.GetEnvironmentVariable(match.Groups["key"].NotNull().Value);
            return (value != null, value);
        }
    }
}