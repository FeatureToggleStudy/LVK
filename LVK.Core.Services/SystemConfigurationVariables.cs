using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    internal class SystemConfigurationVariables : IConfigurationVariables
    {
        [NotNull]
        private Dictionary<string, Func<string>> _SystemConfigurationVariableProviders;

        public string Prefix => "sys";

        public SystemConfigurationVariables(
            [NotNull, ItemNotNull] IEnumerable<ISystemConfigurationVariablesProvider> systemConfigurationVariablesProviders)
        {
            if (systemConfigurationVariablesProviders == null)
                throw new ArgumentNullException(nameof(systemConfigurationVariablesProviders));

            _SystemConfigurationVariableProviders = (
                from systemConfigurationVariablesProvider in systemConfigurationVariablesProviders
                from systemConfigurationVariableProvider in systemConfigurationVariablesProvider.GetSystemConfigurationVariableProviders()
                select systemConfigurationVariableProvider).ToDictionary(scvp => scvp.key, scvp => scvp.getValue);
        }

        public (bool success, string value) TryGetValue(string name)
        {
            if (!_SystemConfigurationVariableProviders.TryGetValue(name, out var getValue))
                return (false, default);

            if (getValue == null)
                return (false, default);

            var value = getValue();
            return (value != null, value);
        }
    }
}