using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

using LVK.Core;

namespace LVK.Logging
{
    [PublicAPI]
    public static class LoggerExtensions
    {
        public static void LogTrace([NotNull] this ILogger logger, [CanBeNull] string message = null,
                                    [CallerFilePath] string callerFilePath = null,
                                    [CallerMemberName] string callerMemberName = null,
                                    [CallerLineNumber] int callerLineNumber = 0)
            => Log(logger, LogLevel.Trace, $"{(!string.IsNullOrWhiteSpace(message) ? message + " @ " : "")}{callerMemberName} @ {callerFilePath} #{callerLineNumber}");

        public static void LogDebug([NotNull] this ILogger logger, [NotNull] string message)
            => Log(logger, LogLevel.Debug, message);

        public static void LogInformation([NotNull] this ILogger logger, [NotNull] string message)
            => Log(logger, LogLevel.Information, message);
        
        public static void LogWarning([NotNull] this ILogger logger, [NotNull] string message)
            => Log(logger, LogLevel.Warning, message);
        
        public static void LogError([NotNull] this ILogger logger, [NotNull] string message)
            => Log(logger, LogLevel.Error, message);
        
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

            Log(logger, LogLevel.Debug, sb.ToString());
        }

        private static void Log([NotNull] ILogger logger, LogLevel logLevel, [NotNull] string message)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            logger.Log(logLevel, message);
        }

        public static IDisposable LogScope([NotNull] this ILogger logger, LogLevel logLevel, [NotNull] string scopeName)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (scopeName == null) throw new ArgumentNullException(nameof(scopeName));

            Stopwatch sw = null;
            void openScope()
            {
                logger.Log(logLevel, $"start: {scopeName}");
                
                sw = Stopwatch.StartNew();
            }

            void closeScope()
            {
                assume(sw != null);
                sw.Stop();

                logger.Log(logLevel, $"end: {scopeName} in {sw.ElapsedMilliseconds} ms");
            }

            return new ActionDisposable(openScope, closeScope);
        }
    }
}