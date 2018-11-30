using System;

using JetBrains.Annotations;

namespace LVK.Processes
{
    [PublicAPI]
    public interface IConsoleProcess
    {
        [PublicAPI]
        void Write([NotNull] string text);

        [PublicAPI]
        void WriteLine([NotNull] string line);

        [PublicAPI]
        void Terminate();

        [PublicAPI]
        TimeSpan ExecutionDuration { get; }

        [PublicAPI]
        int Id { get; }
    }
}