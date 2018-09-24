using JetBrains.Annotations;

namespace LVK.Processes
{
    [PublicAPI]
    public interface IConsoleProcessMonitor
    {
        [PublicAPI]
        void Started([NotNull] IConsoleProcess process);

        [PublicAPI]
        void Exited([NotNull] IConsoleProcess process, int exitCode);

        [PublicAPI]
        void Error([NotNull] IConsoleProcess process, [NotNull] string line);

        [PublicAPI]
        void Output([NotNull] IConsoleProcess process, [NotNull] string line);
    }
}