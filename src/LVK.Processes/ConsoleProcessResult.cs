using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Processes
{
    [PublicAPI]
    public class ConsoleProcessResult
    {
        internal ConsoleProcessResult(int exitCode, [NotNull, ItemNotNull] IEnumerable<string> standardOutput, [NotNull, ItemNotNull] IEnumerable<string> errorOutput)
        {
            if (standardOutput == null)
                throw new ArgumentNullException(nameof(standardOutput));

            if (errorOutput == null)
                throw new ArgumentNullException(nameof(errorOutput));

            ExitCode = exitCode;
            StandardOutput = standardOutput.ToList();
            ErrorOutput = errorOutput.ToList();
        }

        public int ExitCode { get; }

        [NotNull, ItemNotNull]
        public IReadOnlyCollection<string> StandardOutput { get; }

        [NotNull, ItemNotNull]
        public IReadOnlyCollection<string> ErrorOutput { get; }
    }
}