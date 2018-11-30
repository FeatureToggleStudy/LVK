using System;
using System.Text;

using JetBrains.Annotations;

using LVK.Data.Caching;

namespace LVK.Data.Protection
{
    internal class EnvironmentVariableDataProtectionPasswordProvider : IDataProtectionPasswordProvider
    {
        [NotNull]
        private readonly IWeakCache<string, string> _VariableNameCache;

        public EnvironmentVariableDataProtectionPasswordProvider([NotNull] IWeakCache<string, string> variableNameCache)
        {
            _VariableNameCache = variableNameCache ?? throw new ArgumentNullException(nameof(variableNameCache));
        }

        public string TryGetPassword(string passwordName)
        {
            var variableName = _VariableNameCache.GetOrAddValue(passwordName, CalculateVariableName);
            var value = Environment.GetEnvironmentVariable(variableName);
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private string CalculateVariableName([NotNull] string passwordName)
        {
            var sb = new StringBuilder("DATA_PROTECTION_SCOPE_");

            foreach (var c in passwordName)
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' || c <= '9'))
                    sb.Append(c);

            return sb.ToString();
        }
    }
}