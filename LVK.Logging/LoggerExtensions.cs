using System;
using System.Diagnostics;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Logging
{
    public static class LoggerExtensions
    {
        public static void Debug([NotNull] this ILogger logger, [NotNull] string message) => (logger ?? throw new ArgumentNullException(nameof(logger))).Log(LogLevel.Debug, message);
        public static void Verbose([NotNull] this ILogger logger, [NotNull] string message) => (logger ?? throw new ArgumentNullException(nameof(logger))).Log(LogLevel.Verbose, message);
        public static void Information([NotNull] this ILogger logger, [NotNull] string message) => (logger ?? throw new ArgumentNullException(nameof(logger))).Log(LogLevel.Information, message);
        public static void Warning([NotNull] this ILogger logger, [NotNull] string message) => (logger ?? throw new ArgumentNullException(nameof(logger))).Log(LogLevel.Warning, message);
        public static void Error([NotNull] this ILogger logger, [NotNull] string message) => (logger ?? throw new ArgumentNullException(nameof(logger))).Log(LogLevel.Error, message);

        [NotNull]
        public static IDisposable Scope([NotNull] this ILogger logger, LogLevel level, [NotNull] string scopeName)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (scopeName == null) throw new ArgumentNullException(nameof(scopeName));

            var stopwatch = Stopwatch.StartNew();
            logger.Debug($"start: {scopeName}");
            return new ActionDisposable(() =>
            {
                stopwatch.Stop();
                logger.Debug($"end: {scopeName} in {stopwatch.ElapsedMilliseconds} ms");
            });
        }
    }
}