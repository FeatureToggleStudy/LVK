using System.Linq;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Processes.Events;
using LVK.Processes.Monitors;

namespace LVK.Processes
{
    [PublicAPI]
    public static class ConsoleProcessFactoryExtensions
    {
        [NotNull]
        public static Task<int> SpawnAsync([NotNull] this IConsoleProcessFactory factory, [NotNull] string executableFilename)
            => factory.SpawnAsync(executableFilename, new string[0], null, Enumerable.Empty<IConsoleProcessMonitor>());

        [NotNull]
        public static Task<int> SpawnAsync(
            [NotNull] this IConsoleProcessFactory factory, [NotNull] string executableFilename, [NotNull, ItemNotNull] string[] arguments)
            => factory.SpawnAsync(executableFilename, arguments, null, Enumerable.Empty<IConsoleProcessMonitor>());

        [NotNull, ItemNotNull]
        public static Task<ConsoleProcessResult> SpawnAndCollectOutputAsync([NotNull] this IConsoleProcessFactory factory, [NotNull] string executableFilename)
            => SpawnAndCollectOutputAsync(factory, executableFilename, new string[0]);

        [NotNull, ItemNotNull]
        public static Task<ConsoleProcessResult> SpawnAndCollectOutputAsync(
            [NotNull] this IConsoleProcessFactory factory, [NotNull] string executableFilename, [NotNull, ItemNotNull] string[] arguments)
            => SpawnAndCollectOutputAsync(factory, executableFilename, arguments, null);

        [NotNull, ItemNotNull]
        public static async Task<ConsoleProcessResult> SpawnAndCollectOutputAsync(
            [NotNull] this IConsoleProcessFactory factory, [NotNull] string executableFilename, [NotNull, ItemNotNull] string[] arguments,
            [CanBeNull] string workingDirectory)
        {
            var monitor = new ProcessCollectOutputMonitor();

            var exitCode = await factory.SpawnAsync(executableFilename, arguments, workingDirectory, new IConsoleProcessMonitor[] { monitor });

            var standardOutput = monitor.Events.OfType<ConsoleProcessStandardOutputEvent>().Select(evt => evt.Line);
            var errorOutput = monitor.Events.OfType<ConsoleProcessErrorOutputEvent>().Select(evt => evt.Line);

            return new ConsoleProcessResult(exitCode, standardOutput, errorOutput);
        }
    }
}