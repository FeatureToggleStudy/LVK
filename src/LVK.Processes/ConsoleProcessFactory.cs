using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Processes.Monitors;

namespace LVK.Processes
{
    internal class ConsoleProcessFactory : IConsoleProcessFactory
    {
        [NotNull]
        private readonly List<IConsoleProcessMonitor> _DefaultProcessMonitors;

        public ConsoleProcessFactory([NotNull] IEnumerable<IConsoleProcessMonitor> defaultProcessMonitors)
        {
            if (defaultProcessMonitors == null)
                throw new ArgumentNullException(nameof(defaultProcessMonitors));

            _DefaultProcessMonitors = defaultProcessMonitors.ToList();
        }

        public async Task<int> SpawnAsync(
            string executableFilePath, string[] parameters, string workingDirectory, IEnumerable<IConsoleProcessMonitor> monitors)
        {
            if (executableFilePath == null)
                throw new ArgumentNullException(nameof(executableFilePath));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            if (monitors == null)
                throw new ArgumentNullException(nameof(monitors));

            monitors = _DefaultProcessMonitors.Concat(monitors);
            using (var process = new ConsoleProcess(CreateProcessStartInfo(executableFilePath, parameters, workingDirectory), monitors.ToArray()))
            {
                return await process.StartAsync();
            }
        }

        [NotNull]
        private ProcessStartInfo CreateProcessStartInfo([NotNull] string executableFilePath, [NotNull] string[] parameters, [CanBeNull] string workingDirectory)
            => new ProcessStartInfo(executableFilePath)
            {
                Arguments = string.Join(" ", parameters.Select(EscapeParameter)), WorkingDirectory = workingDirectory ?? String.Empty
            };

        [NotNull]
        private string EscapeParameter([NotNull] string parameter)
        {
            if (parameter.Contains(" "))
                return $"\"{parameter}\"";

            return parameter;
        }
    }
}