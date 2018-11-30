using System.Collections.Generic;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Processes
{
    [PublicAPI]
    public interface IConsoleProcessFactory
    {
        [NotNull]
        Task<int> SpawnAsync(
            [NotNull] string executableFilePath, [NotNull, ItemNotNull] string[] parameters, [CanBeNull] string workingDirectory,
            [NotNull, ItemNotNull] IEnumerable<IConsoleProcessMonitor> monitors);
    }
}