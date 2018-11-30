using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

using JetBrains.Annotations;

using LVK.Core;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Logging
{
    [PublicAPI]
    public static class LoggerExtensions
    {
        // ReSharper disable ExplicitCallerInfoArgument
        public static void LogTrace(
            [NotNull] this ILogger logger, [CanBeNull] string message = null, [CallerFilePath, CanBeNull] string callerFilePath = null,
            [CallerMemberName, CanBeNull] string callerMemberName = null, [CallerLineNumber] int callerLineNumber = 0)
            => LogTrace(logger, () => message, callerFilePath, callerMemberName, callerLineNumber);

        // ReSharper restore ExplicitCallerInfoArgument

        public static void LogTrace(
            [NotNull] this ILogger logger, [NotNull] Func<string> getMessage, [CallerFilePath, CanBeNull] string callerFilePath = null,
            [CallerMemberName, CanBeNull] string callerMemberName = null, [CallerLineNumber] int callerLineNumber = 0)
        {
            if (getMessage == null)
                throw new ArgumentNullException(nameof(getMessage));

            Log(
                logger, LogLevel.Trace, () =>
                {
                    string message = getMessage();
                    return $"{(!string.IsNullOrWhiteSpace(message) ? message + " @ " : "")}{callerMemberName} @ {callerFilePath} #{callerLineNumber}";
                });
        }

        public static void LogDebug([NotNull] this ILogger logger, [NotNull] string message) => Log(logger, LogLevel.Debug, message);

        public static void LogDebug([NotNull] this ILogger logger, [NotNull] Func<string> getMessage) => Log(logger, LogLevel.Debug, getMessage);

        public static void LogVerbose([NotNull] this ILogger logger, [NotNull] string message) => Log(logger, LogLevel.Verbose, message);

        public static void LogVerbose([NotNull] this ILogger logger, [NotNull] Func<string> getMessage) => Log(logger, LogLevel.Verbose, getMessage);

        public static void LogInformation([NotNull] this ILogger logger, [NotNull] string message) => Log(logger, LogLevel.Information, message);

        public static void LogInformation([NotNull] this ILogger logger, [NotNull] Func<string> getMessage) => Log(logger, LogLevel.Information, getMessage);

        public static void LogWarning([NotNull] this ILogger logger, [NotNull] string message) => Log(logger, LogLevel.Warning, message);

        public static void LogWarning([NotNull] this ILogger logger, [NotNull] Func<string> getMessage) => Log(logger, LogLevel.Warning, getMessage);

        public static void LogError([NotNull] this ILogger logger, [NotNull] string message) => Log(logger, LogLevel.Error, message);

        public static void LogError([NotNull] this ILogger logger, [NotNull] Func<string> getMessage) => Log(logger, LogLevel.Error, getMessage);

        public static void LogException([NotNull] this ILogger logger, [NotNull] Exception ex)
        {
            var sb = new StringBuilder();

            while (ex != null)
            {
                Type exType = ex.GetType();
                assume(exType != null);

                sb.AppendLine($"{exType.Name}: {ex.Message}");
                if (!string.IsNullOrWhiteSpace((ex.StackTrace)))
                    sb.AppendLine(ex.StackTrace);

                ex = ex.InnerException;
                if (ex != null)
                    sb.AppendLine();
            }

            Log(logger, LogLevel.Error, sb.ToString());
        }

        private static void Log([NotNull] ILogger logger, LogLevel logLevel, [NotNull] string message)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            logger.Log(logLevel, message);
        }

        private static void Log([NotNull] ILogger logger, LogLevel logLevel, [NotNull] Func<string> getMessage)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            logger.Log(logLevel, getMessage);
        }

        public static IDisposable LogScope([NotNull] this ILogger logger, LogLevel logLevel, [NotNull] string scopeName)
            => LogScope(logger, logLevel, () => scopeName);

        public static IDisposable LogScope([NotNull] this ILogger logger, LogLevel logLevel, [NotNull] Func<string> getScopeName)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            if (getScopeName == null)
                throw new ArgumentNullException(nameof(getScopeName));

            Stopwatch sw = null;

            void openScope()
            {
                logger.Log(logLevel, () => $"\x00BB {getScopeName()}");

                sw = Stopwatch.StartNew();
            }

            void closeScope()
            {
                assume(sw != null);
                sw.Stop();

                logger.Log(logLevel, () => $"\x00AB {getScopeName()}, finished in {sw.ElapsedMilliseconds} ms");
            }

            return new ActionDisposable(openScope, closeScope);
        }

    }
}