using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Core.Services
{
    [PublicAPI]
    public interface IApplicationContext
    {
        [NotNull, ItemNotNull]
        IEnumerable<string> CommandLineArguments { get; }
    }
}