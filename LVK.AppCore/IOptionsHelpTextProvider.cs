using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.AppCore
{
    [PublicAPI]
    public interface IOptionsHelpTextProvider
    {
        [NotNull]
        IEnumerable<(IEnumerable<string> paths, string description)> GetHelpText();
    }
}