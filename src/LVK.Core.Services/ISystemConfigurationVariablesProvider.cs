using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface ISystemConfigurationVariablesProvider
    {
        [NotNull]
        IEnumerable<(string key, Func<string> getValue)> GetSystemConfigurationVariableProviders();
    }
}