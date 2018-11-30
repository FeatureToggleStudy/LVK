using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface IOptionsHelpTextProvider
    {
        [NotNull]
        IEnumerable<(IEnumerable<string> paths, bool isConfiguration, string description)> GetHelpText();
    }
}