using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Logging
{
    internal interface ITextLogFormatter
    {
        [NotNull, ItemNotNull]
        IEnumerable<string> Format(LogLevel level, [NotNull] string message);
    }
}