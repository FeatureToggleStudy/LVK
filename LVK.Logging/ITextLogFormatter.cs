using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Logging
{
    [PublicAPI]
    public interface ITextLogFormatter
    {
        [NotNull, ItemNotNull]
        IEnumerable<string> Format(LogLevel level, string message);
    }
}